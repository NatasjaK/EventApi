import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { api } from "../lib/api";
import type { GalleryItem } from "../types";

export default function Gallery() {
  const qc = useQueryClient();

  const items = useQuery({
    queryKey: ["gallery"],
    queryFn: () => api.get<GalleryItem[]>("/api/gallery")
  });

  const add = useMutation({
    mutationFn: (payload: Omit<GalleryItem, "id">) => api.post<GalleryItem>("/api/gallery", payload),
    onSuccess: () => qc.invalidateQueries({ queryKey: ["gallery"] })
  });

  function onSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    const fd = new FormData(e.currentTarget);
    const dto = {
      eventId: fd.get("eventId") ? Number(fd.get("eventId")) : null,
      url: String(fd.get("url") || ""),
      caption: String(fd.get("caption") || "")
    };
    add.mutate(dto);
    e.currentTarget.reset();
  }

  return (
    <div className="container grid">
      <h2>Gallery</h2>

      <form onSubmit={onSubmit} className="card form-row">
        <strong>Add image</strong>
        <div className="grid grid-2">
          <input className="input" name="url" placeholder="Image URL (e.g. /images/gallery/echo-1.jpg)" required />
          <input className="input" name="eventId" type="number" placeholder="Event ID (optional)" />
        </div>
        <input className="input" name="caption" placeholder="Caption (optional)" />
        <div className="form-actions">
          <button className="btn" type="submit" disabled={add.isPending}>Add</button>
          {add.isError && <span style={{color:"crimson"}}>{(add.error as Error).message}</span>}
        </div>
      </form>

      {items.isLoading ? <p>Loading…</p> :
       items.isError   ? <p>Error: {(items.error as Error).message}</p> :
       <div className="grid grid-4">
         {items.data!.map(img => (
           <figure key={img.id} className="card" style={{padding:12}}>
             <img src={img.url} alt={img.caption ?? `Image ${img.id}`} style={{width:"100%", borderRadius:8, objectFit:"cover", aspectRatio:"4/3"}} />
             <figcaption className="muted" style={{marginTop:8}}>
               {img.caption ?? "—"} {img.eventId ? <span className="pill">Event #{img.eventId}</span> : null}
             </figcaption>
           </figure>
         ))}
       </div>}
    </div>
  );
}
