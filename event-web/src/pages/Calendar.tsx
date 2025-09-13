import { useQuery } from "@tanstack/react-query";
import { api } from "../lib/api";

type CalendarEntry = { id:number; eventId:number; title:string; endDate:string };
type EventLite = { id:number; title:string };

export default function CalendarPage() {
  const entries = useQuery({
    queryKey: ["calendarentries"],
    queryFn: () => api.get<CalendarEntry[]>("/api/calendarentries")
  });
  const events = useQuery({
    queryKey: ["events-lite"],
    queryFn: () => api.get<EventLite[]>("/api/events")
  });

  if (entries.isLoading) return <div className="container"><p>Loading…</p></div>;
  if (entries.isError)   return <div className="container"><p>Error: {(entries.error as Error).message}</p></div>;

  const titleById = new Map((events.data ?? []).map(e => [e.id, e.title]));
  const rows = (entries.data ?? []).sort((a,b)=>+new Date(a.endDate)-+new Date(b.endDate));

  return (
    <div className="container grid">
      <h2>Calendar</h2>
      <div className="table-wrap">
        <table className="table">
          <thead><tr><th>Date</th><th>Title</th><th>Event</th></tr></thead>
          <tbody>
            {rows.map(c=>(
              <tr key={c.id}>
                <td>{new Date(c.endDate).toLocaleString()}</td>
                <td>{c.title}</td>
                <td>{titleById.get(c.eventId) ?? `#${c.eventId}`}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
