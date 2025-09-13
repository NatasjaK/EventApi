import { Route, Routes } from "react-router-dom";
import Layout from "./layout/Layout";
import Dashboard from "./pages/Dashboard";
import Events from "./pages/Events";
import EventDetails from "./pages/EventDetails";
import Venues from "./pages/Venues"; 
import CalendarPage from "./pages/Calendar";
import Financials from "./pages/Financials";
import Privacy from "./pages/Privacy";
import Terms from "./pages/Terms";
import Contact from "./pages/Contact";
import Bookings from "./pages/Bookings";

export default function App() {
  return (
    <Routes>
      <Route element={<Layout />}>
        <Route path="/" element={<Dashboard />} />
        <Route path="/events" element={<Events />} />
        <Route path="/events/:id" element={<EventDetails />} />
        <Route path="/venues" element={<Venues />} />
        <Route path="/calendar" element={<CalendarPage />} />
        <Route path="/financials" element={<Financials />} />
        <Route path="/privacy" element={<Privacy />} />
        <Route path="/terms" element={<Terms />} />
        <Route path="/contact" element={<Contact />} />
        <Route path="/bookings" element={<Bookings />} />
      </Route>
    </Routes>
  );
}
