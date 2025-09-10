// AI-generated helper
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { api } from "../lib/api";
import type { EventRead, EventCreate } from "../types";

export default function Events() {
  const qc = useQueryClient();
  const events = useQuery({
    queryKey: ["events"],
    queryFn: () => api.get<EventRead[]>("/api/events")
  });

  const create = useMutation({
    mutationFn: (payload: EventCreate) => api.post<EventRead>("/api/events", payload),
    onSuccess: () => qc.invalidateQueries({ queryKey: ["events"] })
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
      venueId: fd.get("venueId") ? Number(fd.get("venueId")) : undefined
    };
    create.mutate(dto);
    e.currentTarget.reset();
  }

  return (
    <div style={{ display: "grid", gap: 16 }}>
      <h3>Events</h3>

      <form onSubmit={onSubmit} style={{ display: "grid", gap: 8, background: "#fff", padding: 12, borderRadius: 8 }}>
        <strong>Nytt event</strong>
        <input name="title" placeholder="Titel" required />
        <input name="description" placeholder="Beskrivning" />
        <input name="date" type="datetime-local" required />
        <input name="location" placeholder="Plats" />
        <input name="maxSeats" type="number" placeholder="Max platser" required />
        <input name="price" type="number" step="0.01" placeholder="Pris" required />
        <input name="venueId" type="number" placeholder="VenueId (valfritt)" />
        <button type="submit" disabled={create.isPending}>Skapa</button>
        {create.isError && <span style={{ color: "crimson" }}>{(create.error as Error).message}</span>}
      </form>

      {events.isLoading ? <p>Laddar…</p> :
       events.isError ? <p>Fel: {(events.error as Error).message}</p> :
       <table style={{ width: "100%", background: "#fff", borderRadius: 8 }}>
         <thead><tr><th>ID</th><th>Titel</th><th>Datum</th><th>Pris</th><th>Platser</th></tr></thead>
         <tbody>
           {events.data!.map(e => (
             <tr key={e.id}>
               <td>{e.id}</td>
               <td>{e.title}</td>
               <td>{new Date(e.date).toLocaleString()}</td>
               <td>{e.price}</td>
               <td>{e.maxSeats}</td>
             </tr>
           ))}
         </tbody>
       </table>}
    </div>
  );
}
