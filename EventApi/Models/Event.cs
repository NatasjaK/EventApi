namespace EventApi.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public int MaxSeats { get; set; }
        public decimal Price { get; set; }

        public int VenueId { get; set; }
        public Venue Venue { get; set; }

        public ICollection<Booking> Bookings { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
        public ICollection<Invoice> Invoices { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
        public ICollection<Package> Packages { get; set; }
        public ICollection<CalendarEntry> CalendarEntries { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}
