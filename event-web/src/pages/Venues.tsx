// AI-generated with assistance
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { api } from "../lib/api";
import type { VenueRead, VenueCreate } from "../types";

export default function Venues() {
  const qc = useQueryClient();

  const venues = useQuery({
    queryKey: ["venues"],
    queryFn: () => api.get<VenueRead[]>("/api/venues")
  });

  const create = useMutation({
    mutationFn: (payload: VenueCreate) => api.post<VenueRead>("/api/venues", payload),
    onSuccess: () => qc.invalidateQueries({ queryKey: ["venues"] })
  });

  const remove = useMutation({
    mutationFn: (id: number) => api.del(`/api/venues/${id}`),
    onSuccess: () => qc.invalidateQueries({ queryKey: ["venues"] })
  });

  function onSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    const fd = new FormData(e.currentTarget);
    const dto: VenueCreate = {
      name: String(fd.get("name") || ""),
      mapImage: String(fd.get("mapImage") || ""),
      address: String(fd.get("address") || "")
    };
    create.mutate(dto);
    e.currentTarget.reset();
  }

  return (
    <div className="grid" style={{ gap: 16 }}>
      <h2>Venues</h2>

      {/* Create form */}
      <form onSubmit={onSubmit} className="card form-row">
        <strong>Nytt Venue</strong>
        <input className="input" name="name" placeholder="Namn" required />
        <div className="grid grid-2">
          <input className="input" name="mapImage" placeholder="Karta/bild (filnamn/url)" />
          <input className="input" name="address" placeholder="Adress" />
        </div>
        <div className="form-actions">
          <button className="btn" type="submit" disabled={create.isPending}>Skapa</button>
          {create.isError && <span style={{ color: "crimson" }}>{(create.error as Error).message}</span>}
        </div>
      </form>

      {/* List */}
      {venues.isLoading ? (
        <p>Laddar…</p>
      ) : venues.isError ? (
        <p>Fel: {(venues.error as Error).message}</p>
      ) : (
        <div className="card" style={{ padding: 0 }}>
          <table className="table">
            <thead>
              <tr><th>ID</th><th>Namn</th><th>Adress</th><th>Karta</th><th></th></tr>
            </thead>
            <tbody>
              {venues.data!.map(v => (
                <tr key={v.id}>
                  <td>{v.id}</td>
                  <td>{v.name}</td>
                  <td>{v.address ?? "-"}</td>
                  <td>{v.mapImage ?? "-"}</td>
                  <td>
                    <button className="btn btn-outline"
                      onClick={() => { if (confirm(`Ta bort ${v.name}?`)) remove.mutate(v.id); }}>
                      Ta bort
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}
