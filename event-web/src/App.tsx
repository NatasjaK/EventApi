import { Link, Route, Routes, NavLink } from "react-router-dom";
import Dashboard from "./pages/Dashboard";
import Events from "./pages/Events";

export default function App() {
  return (
    <div style={{ maxWidth: 1200, margin: "0 auto", padding: 16 }}>
      <header style={{ display: "flex", gap: 16, alignItems: "center" }}>
        <h2 style={{ marginRight: "auto" }}><Link to="/">Ventize</Link></h2>
        <NavLink to="/" end>Dashboard</NavLink>
        <NavLink to="/events">Events</NavLink>
      </header>
      <hr />
      <Routes>
        <Route path="/" element={<Dashboard />} />
        <Route path="/events" element={<Events />} />
      </Routes>
    </div>
  );
}
