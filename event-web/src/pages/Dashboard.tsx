import { useQuery } from "@tanstack/react-query";
import { api } from "../lib/api";
import type { DashboardSummary, RecentBooking, RevenuePoint, UpcomingEvent } from "../types";
import { format } from "date-fns";
import { LineChart, Line, XAxis, YAxis, Tooltip, ResponsiveContainer } from "recharts";

export default function Dashboard() {
  const summary = useQuery({
    queryKey: ["summary"],
    queryFn: () => api.get<DashboardSummary>("/api/dashboard/summary")
  });

  const recent = useQuery({
    queryKey: ["recent"],
    queryFn: () => api.get<RecentBooking[]>("/api/dashboard/recent-bookings?top=5")
  });

  const revenue = useQuery({
    queryKey: ["revenue"],
    queryFn: () => api.get<RevenuePoint[]>("/api/dashboard/revenue-range")
  });

  const upcoming = useQuery({
    queryKey: ["upcoming"],
    queryFn: () => api.get<UpcomingEvent[]>("/api/dashboard/upcoming?days=30")
  });

  if (summary.isLoading) return <p>Laddar…</p>;
  if (summary.isError)   return <p>Fel: {(summary.error as Error).message}</p>;

  const s = summary.data!;
  return (
    <div style={{ display: "grid", gap: 16 }}>
      <section style={{ display: "grid", gap: 12, gridTemplateColumns: "repeat(4, 1fr)" }}>
        <Card title="Events" value={`${s.totalEvents} (${s.upcomingEvents} kommande)`} />
        <Card title="Kunder" value={s.totalUsers} />
        <Card title="Bokningar" value={s.totalBookings} />
        <Card title="Intäkter" value={`${s.totalRevenue.toFixed(2)} kr`} />
      </section>

      <section style={{ display: "grid", gap: 16, gridTemplateColumns: "2fr 1fr" }}>
        <div style={{ height: 320, background: "#fff", borderRadius: 8, padding: 12 }}>
          <h3>Intäkter (30 dagar)</h3>
          <ResponsiveContainer width="100%" height="100%">
            <LineChart data={revenue.data ?? []}>
              <XAxis dataKey="label" hide />
              <YAxis />
              <Tooltip />
              <Line type="monotone" dataKey="amount" stroke="#8b5cf6" strokeWidth={2} dot={false} />
            </LineChart>
          </ResponsiveContainer>
        </div>

        <div style={{ background: "#fff", borderRadius: 8, padding: 12 }}>
          <h3>Omdömen & inkorg</h3>
          <p>Snittbetyg: <b>{s.avgRating.toFixed(2)}</b></p>
          <p>Olästa meddelanden: <b>{s.unreadMessages}</b></p>
          <p>Sålda biljetter: <b>{s.ticketsSold}</b></p>
        </div>
      </section>

      <section style={{ display: "grid", gap: 16, gridTemplateColumns: "1fr 1fr" }}>
        <div style={{ background: "#fff", borderRadius: 8, padding: 12 }}>
          <h3>Senaste bokningar</h3>
          <ul>
            {(recent.data ?? []).map(b => (
              <li key={b.id}>
                {b.userName} → <b>{b.eventTitle}</b> ({b.tickets} t) – {format(new Date(b.bookingDate), "yyyy-MM-dd")}
              </li>
            ))}
          </ul>
        </div>

        <div style={{ background: "#fff", borderRadius: 8, padding: 12 }}>
          <h3>Kommande event (30 dgr)</h3>
          <ul>
            {(upcoming.data ?? []).map(u => (
              <li key={u.eventId}>
                {format(new Date(u.date), "yyyy-MM-dd")} – <b>{u.title}</b> {u.venueName ? `@ ${u.venueName}` : ""} · {u.spotsLeft} platser kvar
              </li>
            ))}
          </ul>
        </div>
      </section>
    </div>
  );
}

function Card({ title, value }: { title: string; value: React.ReactNode }) {
  return (
    <div style={{ background: "#fff", borderRadius: 8, padding: 12 }}>
      <div style={{ color: "#666" }}>{title}</div>
      <div style={{ fontSize: 24, fontWeight: 700 }}>{value}</div>
    </div>
  );
}
