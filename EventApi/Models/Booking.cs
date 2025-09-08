namespace EventApi.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }

        public DateTime BookingDate { get; set; }
        public string Status { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
    }
}
