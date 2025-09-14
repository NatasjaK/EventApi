import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { api } from "../lib/api";
import type { FeedbackRead, FeedbackCreate } from "../types";

type EventLite = { id:number; title:string };
// högst upp i filen
const fmtDate = (v?: string | null) => (v ? new Date(v).toLocaleString() : "—");


export default function FeedbackPage() {
  const qc = useQueryClient();

  const events = useQuery({
    queryKey: ["events-lite"],
    queryFn: () => api.get<EventLite[]>("/api/events")
  });

  const feedbacks = useQuery({
    queryKey: ["feedbacks-all"],
    queryFn: () => api.get<FeedbackRead[]>("/api/feedbacks")
  });

  const create = useMutation({
    mutationFn: (payload: FeedbackCreate) => api.post<FeedbackRead>("/api/feedbacks", payload),
    onSuccess: () => qc.invalidateQueries({ queryKey: ["feedbacks-all"] })
  });

  function onSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    const fd = new FormData(e.currentTarget);
    const dto: FeedbackCreate = {
      eventId: Number(fd.get("eventId")),
      userId: 1, // demo user
      rating: Number(fd.get("rating")),
      comment: String(fd.get("comment") || "")
    };
    create.mutate(dto);
    e.currentTarget.reset();
  }

  const titleById = new Map((events.data ?? []).map(e => [e.id, e.title]));

  return (
    <div className="container grid">
      <h2>Feedback</h2>

      <form onSubmit={onSubmit} className="card form-row">
        <strong>Leave feedback</strong>
        <div className="grid grid-3">
          <select name="eventId" className="input" required defaultValue="">
            <option value="" disabled>Select event…</option>
            {(events.data ?? []).map(e => (
              <option key={e.id} value={e.id}>{e.title}</option>
            ))}
          </select>
          <input className="input" name="rating" type="number" min={1} max={5} placeholder="Rating (1–5)" required />
          <input className="input" name="comment" placeholder="Comment (optional)" />
        </div>
        <div className="form-actions">
          <button className="btn" type="submit" disabled={create.isPending}>Submit</button>
          {create.isError && <span style={{color:"crimson"}}>{(create.error as Error).message}</span>}
        </div>
      </form>

      <section className="card" style={{padding:0}}>
        <table className="table">
          <thead>
            <tr><th>Event</th><th>Rating</th><th>Comment</th><th>Date</th></tr>
          </thead>
          <tbody>
            {(feedbacks.data ?? []).slice().reverse().map(f => (
              <tr key={f.id}>
                <td>{titleById.get(f.eventId) ?? `#${f.eventId}`}</td>
                <td>{f.rating}</td>
                <td>{f.comment ?? "—"}</td>
                <td>{fmtDate(f.createdAt)}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </section>
    </div>
  );
}
