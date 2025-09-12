namespace EventApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
