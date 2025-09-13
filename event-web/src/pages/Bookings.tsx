import { useMemo, useState } from "react";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { api } from "../lib/api";
import type { BookingRead, BookingCreate, EventRead, UserRead, RecentBooking } from "../types";
import { Link } from "react-router-dom";

const fmtDateTime = (s: string) => new Date(s).toLocaleString();
const fmtNumber = (n: number) => n.toLocaleString("sv-SE");

type StatusFilter = "ALL" | "Confirmed" | "Pending" | "Cancelled";

export default function Bookings() {
  const qc = useQueryClient();
  const [search, setSearch] = useState("");
  const [status, setStatus] = useState<StatusFilter>("ALL");

  const bookings = useQuery({
    queryKey: ["bookings"],
    queryFn: () => api.get<BookingRead[]>("/api/bookings"),
    retry: 0
  });

  const recent = useQuery({
    queryKey: ["bookings-fallback"],
    enabled: bookings.isError,
    queryFn: () => api.get<RecentBooking[]>("/api/dashboard/recent-bookings?top=100")
  });

  const events = useQuery({
    queryKey: ["events-lite"],
    queryFn: () => api.get<EventRead[]>("/api/events")
  });

  const users = useQuery({
    queryKey: ["users-lite"],
    queryFn: () => api.get<UserRead[]>("/api/users")
  });

  const create = useMutation({
    mutationFn: (payload: BookingCreate) => api.post<BookingRead>("/api/bookings", payload),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ["bookings"] });
      alert("Booking created");
    }
  });

  const remove = useMutation({
    mutationFn: (id: number) => api.del(`/api/bookings/${id}`),
    onSuccess: () => qc.invalidateQueries({ queryKey: ["bookings"] })
  });

  const rows: BookingRead[] = useMemo(() => {
    if (bookings.data) return bookings.data;

    if (recent.data) {
      return recent.data.map((r) => ({
        id: r.id,
        eventId: 0,
        eventTitle: r.eventTitle,
        userId: 0,
        userName: r.userName,
        bookingDate: r.bookingDate,
        status: r.status ?? "Confirmed",
        tickets: r.tickets ? Array.from({ length: r.tickets }).map((_, i) => ({ id: i + 1 })) : []
      }));
    }
    return [];
  }, [bookings.data, recent.data]);

  const filtered = rows.filter((b) => {
    const txt = `${b.userName ?? ""} ${b.eventTitle ?? ""}`.toLowerCase();
    const matchSearch = txt.includes(search.toLowerCase());
    const matchStatus = status === "ALL" ? true : (b.status || "").toLowerCase() === status.toLowerCase();
    return matchSearch && matchStatus;
  });

  function onSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    const fd = new FormData(e.currentTarget);
    const dto: BookingCreate = {
      eventId: Number(fd.get("eventId")),
      userId: Number(fd.get("userId")),
      status: String(fd.get("status") || "Confirmed"),
    };
    create.mutate(dto);
    e.currentTarget.reset();
  }

  return (
    <div className="grid">
      <div className="card">
        <div className="card-header">
          <h2 style={{ margin: 0 }}>Bookings</h2>
        </div>

        <div className="grid" style={{ gap: 12, marginBottom: 12 }}>
          <div className="grid grid-3">
            <input
              className="input"
              placeholder="Search by user or event…"
              value={search}
              onChange={(e) => setSearch(e.target.value)}
            />
            <select className="input" value={status} onChange={(e) => setStatus(e.target.value as StatusFilter)}>
              <option value="ALL">All statuses</option>
              <option value="Confirmed">Confirmed</option>
              <option value="Pending">Pending</option>
              <option value="Cancelled">Cancelled</option>
            </select>
            <div className="pill">Total: {fmtNumber(filtered.length)}</div>
          </div>
        </div>

        <div className="table-wrap">
          <table className="table">
            <thead>
              <tr>
                <th>ID</th>
                <th>User</th>
                <th>Event</th>
                <th>Date</th>
                <th>Tickets</th>
                <th>Status</th>
                <th className="right">Actions</th>
              </tr>
            </thead>
            <tbody>
              {bookings.isLoading && (
                <tr><td colSpan={7}>Loading…</td></tr>
              )}
              {bookings.isError && !recent.data && (
                <tr><td colSpan={7} style={{ color: "crimson" }}>
                  Could not fetch <code>/api/bookings</code>. Showing nothing until fallback is ready.
                </td></tr>
              )}
              {filtered.map((b) => (
                <tr key={b.id}>
                  <td>{b.id}</td>
                  <td>{b.userName ?? b.userId}</td>
                  <td>
                    {b.eventTitle ? (
                      <Link to={`/events/${b.id}`} style={{ fontWeight: 600 }}>{b.eventTitle}</Link>
                    ) : (
                      b.eventId
                    )}
                  </td>
                  <td>{fmtDateTime(b.bookingDate)}</td>
                  <td>{b.tickets?.length ?? 0}</td>
                  <td>
                    <span className="badge" style={badgeStyle(b.status)}>{b.status}</span>
                  </td>
                  <td className="right">
                    <button
                      className="btn btn-outline"
                      onClick={() => remove.mutate(b.id)}
                      disabled={remove.isPending}
                    >
                      Delete
                    </button>
                  </td>
                </tr>
              ))}
              {!bookings.isLoading && filtered.length === 0 && (
                <tr><td colSpan={7}>No bookings found.</td></tr>
              )}
            </tbody>
          </table>
        </div>
      </div>

      <form onSubmit={onSubmit} className="card form-row">
        <strong>Create booking</strong>

        <div className="grid grid-2">
          <label>
            <div className="muted" style={{ marginBottom: 6 }}>Event</div>
            <select name="eventId" className="input" required defaultValue="">
              <option value="" disabled>Select event…</option>
              {(events.data ?? []).map(e => (
                <option key={e.id} value={e.id}>{e.title}</option>
              ))}
            </select>
          </label>

          <label>
            <div className="muted" style={{ marginBottom: 6 }}>User</div>
            <select name="userId" className="input" required defaultValue="">
              <option value="" disabled>Select user…</option>
              {(users.data ?? []).map(u => (
                <option key={u.id} value={u.id}>{u.name}</option>
              ))}
            </select>
          </label>
        </div>

        <div className="grid grid-3">
          <label>
            <div className="muted" style={{ marginBottom: 6 }}>Status</div>
            <select name="status" className="input" defaultValue="Confirmed">
              <option value="Confirmed">Confirmed</option>
              <option value="Pending">Pending</option>
              <option value="Cancelled">Cancelled</option>
            </select>
          </label>
        </div>

        <div className="form-actions">
          <button type="submit" className="btn" disabled={create.isPending}>Create</button>
          {create.isError && <span style={{ color: "crimson" }}>{(create.error as Error).message}</span>}
        </div>
      </form>
    </div>
  );
}

function badgeStyle(status?: string): React.CSSProperties {
  const s = (status ?? "").toLowerCase();
  if (s === "confirmed") return { background: "#ecfdf5", color: "#065f46", border: "1px solid #a7f3d0" };
  if (s === "pending")   return { background: "#fffbeb", color: "#92400e", border: "1px solid #fde68a" };
  if (s === "cancelled") return { background: "#fef2f2", color: "#991b1b", border: "1px solid #fecaca" };
  return {};
}
