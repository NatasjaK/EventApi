namespace EventApi.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public Booking Booking { get; set; }

        public string QRCode { get; set; }
        public string Status { get; set; }
    }
}
