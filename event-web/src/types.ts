
export type DashboardSummary = {
  totalEvents: number; upcomingEvents: number; totalUsers: number;
  totalBookings: number; ticketsSold: number; totalRevenue: number;
  avgRating: number; unreadMessages: number;
};

export type RecentBooking = {
  id: number; userName: string; eventTitle: string;
  bookingDate: string; tickets: number; status: string;
};

export type RevenuePoint = { label: string; amount: number };

export type UpcomingEvent = {
  eventId: number; title: string; date: string;
  venueName?: string | null; maxSeats: number; ticketsSold: number; spotsLeft: number;
};

export type EventRead = {
  id: number; title: string; description?: string | null;
  date: string; location?: string | null; maxSeats: number; price: number; venueId?: number | null;
};

export type EventCreate = {
  title: string; description?: string; date: string; location?: string;
  maxSeats: number; price: number; venueId?: number;
};
