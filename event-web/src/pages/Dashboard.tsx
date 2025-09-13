import { useQuery } from "@tanstack/react-query";
import { api } from "../lib/api";
import type { DashboardSummary, RecentBooking, RevenuePoint, UpcomingEvent, EventRead } from "../types";
import { ResponsiveContainer, LineChart, Line, XAxis, YAxis, Tooltip } from "recharts";
import { t } from "../ui/text";
import { formatMoney } from "../lib/format";

function StatCard({ title, value, sub }: { title: string; value: React.ReactNode; sub?: string }) {
  return (
    <div className="card stat">
      <div className="stat-title">{title}</div>
      <div className="stat-value">{value}</div>
      {sub && <div className="muted" style={{ fontSize: 12 }}>{sub}</div>}
    </div>
  );
}

export default function Dashboard() {
  const summary = useQuery({
    queryKey: ["summary"],
    queryFn: () => api.get<DashboardSummary>("/api/dashboard/summary"),
  });

  const recent = useQuery({
    queryKey: ["recent"],
    queryFn: () => api.get<RecentBooking[]>("/api/dashboard/recent-bookings?top=5"),
  });

  const revenue = useQuery({
    queryKey: ["revenue"],
    queryFn: () => api.get<RevenuePoint[]>("/api/dashboard/revenue-range"),
  });

  const upcoming = useQuery({
    queryKey: ["upcoming"],
    queryFn: () => api.get<UpcomingEvent[]>("/api/dashboard/upcoming?days=30"),
  });

  const events = useQuery({
    queryKey: ["events"],
    queryFn: () => api.get<EventRead[]>("/api/events"),
  });

  if (summary.isLoading) return <p>Loading…</p>;
  if (summary.isError) return <p>Error: {(summary.error as Error).message}</p>;

  const s = summary.data!;
  const totalCapacity = (events.data ?? []).reduce((acc, e) => acc + (e.maxSeats || 0), 0);
  const soldPct = totalCapacity > 0 ? Math.round((s.ticketsSold / totalCapacity) * 100) : 0;

  return (
    <div className="grid">
      {}
      <section className="grid grid-4">
        <StatCard title={t.dashboard.events}    value={`${s.totalEvents}`} sub={`${s.upcomingEvents} ${t.dashboard.upcomingSuffix}`} />
        <StatCard title={t.dashboard.customers} value={s.totalUsers} />
        <StatCard title={t.dashboard.bookings}  value={s.totalBookings} />
        <StatCard title={t.dashboard.revenue}   value={formatMoney(s.totalRevenue)} />
      </section>

      {}
      <section className="grid grid-2">
        <div className="chart">
          <h3 style={{ marginTop: 0, marginBottom: 8 }}>{t.dashboard.salesRevenue}</h3>
          <ResponsiveContainer width="100%" height="85%">
            <LineChart data={revenue.data ?? []}>
              <XAxis dataKey="label" hide />
              <YAxis />
              <Tooltip />
              <Line type="monotone" dataKey="amount" stroke="#8b5cf6" strokeWidth={2} dot={false} />
            </LineChart>
          </ResponsiveContainer>
        </div>

        <div className="card">
          <h3 style={{ marginTop: 0 }}>{t.dashboard.ticketSales}</h3>
          <p className="muted">Sold tickets: <b>{s.ticketsSold}</b></p>
          <p className="muted">Capacity (all events): <b>{totalCapacity}</b></p>
          <p className="muted">Utilization: <b>{soldPct}%</b></p>

          <hr className="sep" />

          <h3 style={{ marginTop: 0 }}>{t.dashboard.inboxRatings}</h3>
          <p className="muted">Average rating: <b>{s.avgRating.toFixed(2)}</b></p>
          <p className="muted">Unread messages: <b>{s.unreadMessages}</b></p>
        </div>
      </section>

      {}
      <section className="grid grid-2">
        <div className="card" style={{ padding: 0 }}>
          <div style={{ padding: "14px var(--space-5)" }}>
            <h3 style={{ margin: 0 }}>{t.dashboard.recentBookings}</h3>
          </div>
          <div className="table-wrap">
            <table className="table">
              <thead>
                <tr><th>User</th><th>Event</th><th>Date</th><th>Tickets</th><th>Status</th></tr>
              </thead>
              <tbody>
                {(recent.data ?? []).map(b => (
                  <tr key={b.id}>
                    <td>{b.userName}</td>
                    <td>{b.eventTitle}</td>
                    <td>{new Date(b.bookingDate).toLocaleDateString("en-GB")}</td>
                    <td>{b.tickets}</td>
                    <td>{b.status}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>

        <div className="card">
          <h3 style={{ marginTop: 0 }}>{t.dashboard.upcomingEvent}</h3>
          {(upcoming.data ?? []).length ? (
            <ul style={{ marginTop: 8, paddingLeft: 18 }}>
              {(upcoming.data ?? []).map(u => (
                <li key={u.eventId}>
                  {new Date(u.date).toLocaleDateString("en-GB")} – <b>{u.title}</b>
                  {u.venueName ? ` @ ${u.venueName}` : ""} · {u.spotsLeft} spots left
                </li>
              ))}
            </ul>
          ) : (
            <p className="muted">{t.dashboard.noUpcoming}</p>
          )}
        </div>
      </section>
    </div>
  );
}
