import { NavLink } from "react-router-dom";

function Item({
  to, label, end = false, icon,
}: { to: string; label: string; end?: boolean; icon: React.ReactNode }) {
  return (
    <NavLink to={to} end={end} className={({ isActive }) => (isActive ? "active" : "")}>
      {icon}<span>{label}</span>
    </NavLink>
  );
}

export default function Sidebar() {
  return (
    <aside className="sidebar">
      <NavLink to="/" className="brand">Ventize</NavLink>

      <nav className="snav" aria-label="Main">
        <Item to="/" end label="Dashboard" icon={
          <svg width="18" height="18" viewBox="0 0 24 24" fill="none">
            <path d="M3 12l9-9 9 9" stroke="currentColor" strokeWidth="2" />
            <path d="M9 21V12h6v9" stroke="currentColor" strokeWidth="2" />
          </svg>
        }/>
        <Item to="/events" label="Events" icon={
          <svg width="18" height="18" viewBox="0 0 24 24" fill="none">
            <path d="M3 8h18M8 3v6M16 3v6M5 12h14v9H5z" stroke="currentColor" strokeWidth="2" />
          </svg>
        }/>
        <Item to="/venues" label="Venues" icon={
          <svg width="18" height="18" viewBox="0 0 24 24" fill="none">
            <path d="M12 2l7 6v12H5V8l7-6z" stroke="currentColor" strokeWidth="2" />
          </svg>
        }/>

        <div className="sep" />

        <Item to="/calendar" label="Calendar" icon={
          <svg width="18" height="18" viewBox="0 0 24 24" fill="none">
            <path d="M7 2v4M17 2v4M3 8h18M5 12h14v8H5z" stroke="currentColor" strokeWidth="2"/>
          </svg>
        }/>
        <Item to="/financials" label="Financials" icon={
          <svg width="18" height="18" viewBox="0 0 24 24" fill="none">
            <path d="M4 20h16M6 16h3v4H6zM11 12h3v8h-3zM16 8h3v12h-3z" stroke="currentColor" strokeWidth="2"/>
          </svg>
        }/>

        <div className="sep" />

        <Item to="/bookings" label="Bookings" icon={
          <svg width="18" height="18" viewBox="0 0 24 24" fill="none">
            <path d="M6 2h12v6H6z" stroke="currentColor" strokeWidth="2"/>
            <path d="M4 8h16v12H4z" stroke="currentColor" strokeWidth="2"/>
          </svg>
        }/>
        <Item to="/gallery" label="Gallery" icon={
          <svg width="18" height="18" viewBox="0 0 24 24" fill="none">
            <rect x="3" y="5" width="18" height="14" rx="2" stroke="currentColor" strokeWidth="2"/>
            <path d="M7 15l3-3 3 3 4-4 3 3" stroke="currentColor" strokeWidth="2"/>
          </svg>
        }/>
        <Item to="/feedback" label="Feedback" icon={
          <svg width="18" height="18" viewBox="0 0 24 24" fill="none">
            <path d="M8 13h8M8 9h10" stroke="currentColor" strokeWidth="2"/>
            <path d="M21 12a9 9 0 11-4-7.6L22 4l-1 4.6" stroke="currentColor" strokeWidth="2"/>
          </svg>
        }/>
      </nav>
    </aside>
  );
}
