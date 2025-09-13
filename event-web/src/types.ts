
export type DashboardSummary = {
  totalEvents: number; upcomingEvents: number; totalUsers: number;
  totalBookings: number; ticketsSold: number; totalRevenue: number;
  avgRating: number; unreadMessages: number;
};

export type RevenuePoint = { label: string; amount: number };

export type UpcomingEvent = {
  eventId: number; title: string; date: string;
  venueName?: string | null; maxSeats: number; ticketsSold: number; spotsLeft: number;
};


export type EventCreate = {
  title: string; description?: string; date: string; location?: string;
  maxSeats: number; price: number; venueId?: number;
};

export type EventFull = {
  id: number;
  title: string;
  description?: string | null;
  date: string;
  location?: string | null;
  price: number;
  maxSeats: number;
  venue?: { id: number; name: string } | null;
  packages: { id: number; title: string; price: number }[];
  calendarEntries: { id: number; title: string; endDate: string }[];
};

export type VenueRead = {
  id: number;
  name: string;
  mapImage?: string | null;
  address?: string | null;
};

export type VenueCreate = {
  name: string;
  mapImage?: string;
  address?: string;
};

export type FeedbackRead = {
  id: number;
  eventId: number;
  userId?: number | null;
  rating: number;
  comment?: string | null;
  createdAt?: string | null;
};

export type FeedbackCreate = {
  eventId: number;
  userId?: number;    
  rating: number;      
  comment?: string;
};
export type BookingRead = {
  id: number;
  eventId: number;
  userId: number;
  bookingDate: string;
  status: string;
  tickets?: { id: number; price?: number }[];
  // berikade namn om ditt API redan skickar med dem:
  eventTitle?: string;
  userName?: string;
};

export type BookingCreate = {
  eventId: number;
  userId: number;
  status?: string;
  bookingDate?: string;
};

export type RecentBooking = {
  id: number;
  userName: string;
  eventTitle: string;
  bookingDate: string;
  tickets: number;
  status?: string;
};

export type EventRead = {
  id: number;
  title: string;
  date: string;
  price: number;
  maxSeats: number;
};

export type UserRead = {
  id: number;
  name: string;
  email?: string;
};

export type GalleryItem = {
  id: number;
  eventId?: number | null;
  url: string;
  caption?: string | null;
};
