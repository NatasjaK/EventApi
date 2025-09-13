import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { api } from "../lib/api";
import type { EventRead, EventCreate } from "../types";
import { Link } from "react-router-dom";
import { t } from "../ui/text";
import { formatDateTime, formatMoney } from "../lib/format";

export default function Events() {
  const qc = useQueryClient();

  const events = useQuery({
    queryKey: ["events"],
    queryFn: () => api.get<EventRead[]>("/api/events"),
  });

  const create = useMutation({
    mutationFn: (payload: EventCreate) => api.post<EventRead>("/api/events", payload),
    onSuccess: () => qc.invalidateQueries({ queryKey: ["events"] }),
  });

  function onSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    const fd = new FormData(e.currentTarget);
    const dto: EventCreate = {
      title: String(fd.get("title") || ""),
      description: String(fd.get("description") || ""),
      date: String(fd.get("date") || ""),
      location: String(fd.get("location") || ""),
      maxSeats: Number(fd.get("maxSeats") || 0),
      price: Number(fd.get("price") || 0),
      venueId: fd.get("venueId") ? Number(fd.get("venueId")) : undefined,
    };
    create.mutate(dto);
    e.currentTarget.reset();
  }

  return (
    <div className="grid">
      <h2>{t.events.title}</h2>

      {}
      <form onSubmit={onSubmit} className="card form-row">
        <strong style={{ marginBottom: 6 }}>{t.events.new}</strong>

        <input className="input" name="title" placeholder={t.events.fields.title} required />
        <input className="input" name="description" placeholder={t.events.fields.description} />
        <input className="input" name="date" type="datetime-local" aria-label={t.events.fields.date} required />
        <input className="input" name="location" placeholder={t.events.fields.location} />

        <div className="grid grid-2">
          <input className="input" name="maxSeats" type="number" placeholder={t.events.fields.maxSeats} required />
          <input className="input" name="price" type="number" step="0.01" placeholder={t.events.fields.price} required />
        </div>

        <input className="input" name="venueId" type="number" placeholder={t.events.fields.venueId} />

        <div className="form-actions">
          <button type="submit" className="btn" disabled={create.isPending}>{t.events.create}</button>
          {create.isError && <span style={{ color: "crimson" }}>{(create.error as Error).message}</span>}
        </div>
      </form>

      {}
      {!events.isLoading && !events.isError && (
        <div className="grid grid-4">
          {(events.data ?? []).slice(0, 8).map(e => (
            <div key={e.id} className="card">
              <div style={{ fontWeight: 600 }}>{e.title}</div>
              <div className="muted">{new Date(e.date).toLocaleDateString("en-GB")}</div>
              <div style={{ marginTop: 8 }}>{formatMoney(e.price)}</div>
              <Link to={`/events/${e.id}`} className="btn" style={{ marginTop: 12, display: "inline-block" }}>
                Details
              </Link>
            </div>
          ))}
        </div>
      )}

      {}
      {events.isLoading ? (
        <p>Loading…</p>
      ) : events.isError ? (
        <p>Error: {(events.error as Error).message}</p>
      ) : (
        <div className="card" style={{ padding: 0 }}>
          <table className="table">
            <thead>
              <tr><th>ID</th><th>{t.events.fields.title}</th><th>{t.events.fields.date}</th><th>{t.events.fields.price}</th><th>{t.events.fields.maxSeats}</th></tr>
            </thead>
            <tbody>
              {events.data!.map(e => (
                <tr key={e.id}>
                  <td>{e.id}</td>
                  <td><Link to={`/events/${e.id}`}>{e.title}</Link></td>
                  <td>{formatDateTime(e.date)}</td>
                  <td>{formatMoney(e.price)}</td>
                  <td>{e.maxSeats}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}
