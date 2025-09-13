
import { Link, useParams } from "react-router-dom";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { api } from "../lib/api";
import type { EventFull, FeedbackRead, FeedbackCreate } from "../types";
import { t } from "../ui/text";
import { formatDateTime, formatMoney } from "../lib/format";

export default function EventDetails() {
  const { id } = useParams();
  const eventId = Number(id);
  const qc = useQueryClient();

  
  const qEvent = useQuery({
    queryKey: ["event", eventId],
    queryFn: () => api.get<EventFull>(`/api/events/${eventId}`),
    enabled: Number.isFinite(eventId),
  });

  const qFeedbacks = useQuery({
    queryKey: ["feedbacks", eventId],
    queryFn: () => api.get<FeedbackRead[]>(`/api/feedbacks?eventId=${eventId}`),
    enabled: Number.isFinite(eventId),
  });

 
  const addFeedback = useMutation({
    mutationFn: (payload: FeedbackCreate) => api.post<FeedbackRead>("/api/feedbacks", payload),
    onSuccess: () => qc.invalidateQueries({ queryKey: ["feedbacks", eventId] }),
  });

  function submitFeedback(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    const fd = new FormData(e.currentTarget);
    const dto: FeedbackCreate = {
      eventId,
      userId: 1, 
      rating: Number(fd.get("rating")),
      comment: String(fd.get("comment") || ""),
    };
    addFeedback.mutate(dto);
    e.currentTarget.reset();
  }

  
  if (!Number.isFinite(eventId)) return <div className="container"><p>Invalid event id.</p></div>;
  if (qEvent.isLoading) return <div className="container"><p>Loading…</p></div>;
  if (qEvent.isError) return <div className="container"><p>Error: {(qEvent.error as Error).message}</p></div>;

  const ev = qEvent.data!;

  return (
    <div className="grid grid-2">
      {}
      <div className="card">
        <div className="header" style={{ marginBottom: 8 }}>
          <Link to="/events" className="btn btn-outline">← {t.eventDetails.back}</Link>
          <h2 style={{ margin: 0 }}>{ev.title}</h2>
        </div>

        <p className="muted" style={{ marginBottom: 8 }}>
          {formatDateTime(ev.date)}
          {ev.location ? ` · ${ev.location}` : ""}
          {ev.venue ? ` · ${ev.venue.name}` : ""}
        </p>

        {ev.description && <p style={{ marginBottom: 12 }}>{ev.description}</p>}

        <div style={{ marginTop: 8, marginBottom: 16 }}>
          <strong>{t.eventDetails.basePrice}:</strong> {formatMoney(ev.price)} ·{" "}
          <strong>{t.eventDetails.seats}:</strong> {ev.maxSeats}
        </div>

        {}
        <h3 style={{ marginTop: 0 }}>{t.eventDetails.packages}</h3>
        <div className="grid grid-2" style={{ marginBottom: 16 }}>
          {ev.packages?.length ? (
            ev.packages.map(p => (
              <div key={p.id} className="card" style={{ padding: 12 }}>
                <div style={{ fontWeight: 600 }}>{p.title}</div>
                <div>{formatMoney(p.price)}</div>
                <button
                  className="btn"
                  style={{ marginTop: 8 }}
                  onClick={() => alert(`(Demo) Booking ${p.title}`)}
                >
                  Book
                </button>
              </div>
            ))
          ) : (
            <div className="muted">{t.eventDetails.noPackages}</div>
          )}
        </div>

        {}
        <h3>{t.eventDetails.schedule}</h3>
        {ev.calendarEntries?.length ? (
          <ul style={{ margin: 0, paddingLeft: 18 }}>
            {ev.calendarEntries.map(c => (
              <li key={c.id}>
                {formatDateTime(c.endDate)} – {c.title}
              </li>
            ))}
          </ul>
        ) : (
          <p className="muted">{t.eventDetails.noSchedule}</p>
        )}

        <hr className="sep" />

        {}
        <h3>{t.eventDetails.ratings}</h3>
        {qFeedbacks.isLoading ? (
          <p>Loading reviews…</p>
        ) : qFeedbacks.isError ? (
          <p>Error loading reviews.</p>
        ) : qFeedbacks.data!.length === 0 ? (
          <p className="muted">No reviews yet.</p>
        ) : (
          <ul style={{ marginTop: 8, paddingLeft: 18 }}>
            {qFeedbacks.data!.map(f => (
              <li key={f.id}>
                <b>{f.rating}</b>/5 — {f.comment ?? "(no message)"}
                {f.createdAt ? (
                  <span className="muted"> · {new Date(f.createdAt).toLocaleDateString("en-GB")}</span>
                ) : null}
              </li>
            ))}
          </ul>
        )}

        {}
        <form onSubmit={submitFeedback} className="card form-row" style={{ marginTop: 12 }}>
          <strong>{t.eventDetails.leaveReview}</strong>
          <div className="grid grid-2">
            <input
              className="input"
              name="rating"
              type="number"
              min={1}
              max={5}
              placeholder={t.eventDetails.rating}
              required
            />
            <input className="input" name="comment" placeholder={t.eventDetails.comment} />
          </div>
          <div className="form-actions">
            <button className="btn" type="submit" disabled={addFeedback.isPending}>
              {t.eventDetails.send}
            </button>
            {addFeedback.isError && (
              <span style={{ color: "crimson" }}>{(addFeedback.error as Error).message}</span>
            )}
          </div>
        </form>
      </div>

      {}
      <div className="card">
        <h3 style={{ marginTop: 0 }}>{t.eventDetails.quickInfo}</h3>
        <ul style={{ marginTop: 8 }}>
          <li>Event ID: {ev.id}</li>
          <li>{t.eventDetails.venue}: {ev.venue?.name ?? "-"}</li>
          <li>{t.eventDetails.seats}: {ev.maxSeats}</li>
        </ul>
        <p className="muted" style={{ marginTop: 8 }}>
          Note: booking/QR/checkout is mocked in this MVP.
        </p>
      </div>
    </div>
  );
}
