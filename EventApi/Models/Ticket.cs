namespace EventApi.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public Booking? Booking { get; set; }
        public int? EventId { get; set; }
        public Event? Event { get; set; }

        public string? SeatNumber { get; set; } 
        public decimal Price { get; set; }

        public string QRCode { get; set; } = Guid.NewGuid().ToString("N");
        public string Status { get; set; }
    }
}
