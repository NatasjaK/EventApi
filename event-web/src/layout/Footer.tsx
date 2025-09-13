import { NavLink } from "react-router-dom";

export default function Footer() {
  const year = new Date().getFullYear();

  return (
    <>
      <footer className="footer">
        <div className="container footer-inner">
          <span>© {year} Ventize</span>

          <nav className="footer-links" aria-label="Legal">
            <NavLink to="/privacy">Privacy Policy</NavLink>
            <NavLink to="/terms">Terms &amp; Conditions</NavLink>
            <NavLink to="/contact">Contact</NavLink>
          </nav>
        </div>
      </footer>

      <nav className="footbar-mobile" aria-label="Primary">
        <NavLink to="/dashboard" className="foot-item">
          <svg width="18" height="18" viewBox="0 0 24 24" fill="none">
            <path d="M3 12l9-9 9 9" stroke="currentColor" strokeWidth="2" />
            <path d="M9 21V12h6v9" stroke="currentColor" strokeWidth="2" />
          </svg>
          <span>Dashboard</span>
        </NavLink>
        <NavLink to="/events" className="foot-item">
          <svg width="18" height="18" viewBox="0 0 24 24" fill="none">
            <path d="M3 8h18M8 3v6M16 3v6M5 12h14v9H5z" stroke="currentColor" strokeWidth="2" />
          </svg>
          <span>Events</span>
        </NavLink>
        <NavLink to="/venues" className="foot-item">
          <svg width="18" height="18" viewBox="0 0 24 24" fill="none">
            <path d="M12 2l7 6v12H5V8l7-6z" stroke="currentColor" strokeWidth="2" />
          </svg>
          <span>Venues</span>
        </NavLink>
      </nav>
    </>
  );
}
