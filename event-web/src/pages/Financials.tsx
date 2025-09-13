import { useQuery } from "@tanstack/react-query";
import { api } from "../lib/api";
import { ResponsiveContainer, LineChart, Line, XAxis, YAxis, Tooltip } from "recharts";

type RevenuePoint = { label:string; amount:number };
type Summary = { totalRevenue:number; ticketsSold:number; totalBookings:number; avgRating:number };
type Tx = { id:number; eventId:number; amount:number; date:string };

const kr = (n:number)=> n.toLocaleString("sv-SE",{style:"currency",currency:"SEK"});

export default function Financials() {
  const summary = useQuery({
    queryKey: ["summary"],
    queryFn: () => api.get<Summary>("/api/dashboard/summary")
  });
  const revenue = useQuery({
    queryKey: ["revenue-30d"],
    queryFn: () => api.get<RevenuePoint[]>("/api/dashboard/revenue-range")
  });
  const tx = useQuery({
    queryKey: ["transactions"],
    queryFn: () => api.get<Tx[]>("/api/transactions")
  });

  if (summary.isLoading || revenue.isLoading) return <div className="container"><p>Loading…</p></div>;
  if (summary.isError) return <div className="container"><p>Error: {(summary.error as Error).message}</p></div>;

  const s = summary.data!;
  return (
    <div className="container grid">
      <h2>Financials</h2>

      <section className="grid grid-4">
        <div className="card stat"><div className="stat-title">Total Revenue</div><div className="stat-value">{kr(s.totalRevenue)}</div></div>
        <div className="card stat"><div className="stat-title">Tickets Sold</div><div className="stat-value">{s.ticketsSold}</div></div>
        <div className="card stat"><div className="stat-title">Bookings</div><div className="stat-value">{s.totalBookings}</div></div>
        <div className="card stat"><div className="stat-title">Avg. Rating</div><div className="stat-value">{s.avgRating.toFixed(2)}</div></div>
      </section>

      <section className="chart">
        <h3 style={{marginTop:0, marginBottom:8}}>Revenue (last 30 days)</h3>
        <ResponsiveContainer width="100%" height="85%">
          <LineChart data={revenue.data ?? []}>
            <XAxis dataKey="label" hide />
            <YAxis />
            <Tooltip />
            <Line type="monotone" dataKey="amount" stroke="#8b5cf6" strokeWidth={2} dot={false} />
          </LineChart>
        </ResponsiveContainer>
      </section>

      <section className="card">
        <h3 style={{marginTop:0}}>Recent Transactions</h3>
        {!tx.isLoading && !tx.isError ? (
          <div className="table-wrap" style={{marginTop:8}}>
            <table className="table">
              <thead><tr><th>ID</th><th>Event</th><th>Amount</th><th>Date</th></tr></thead>
              <tbody>
                {(tx.data ?? []).slice(0,50).map(t=>(
                  <tr key={t.id}>
                    <td>{t.id}</td>
                    <td>{t.eventId}</td>
                    <td>{kr(t.amount)}</td>
                    <td>{new Date(t.date).toLocaleString()}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        ) : <p className="muted">Loading transactions…</p>}
      </section>
    </div>
  );
}
