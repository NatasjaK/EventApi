namespace EventApi.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public int MaxSeats { get; set; }
        public decimal Price { get; set; }

        public int? VenueId { get; set; }
        public Venue? Venue { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
        public ICollection<Package> Packages { get; set; } = new List<Package>();
        public ICollection<CalendarEntry> CalendarEntries { get; set; } = new List<CalendarEntry>();
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
