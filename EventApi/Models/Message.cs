namespace EventApi.Models
{
    public class Message
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public string Title { get; set; }
        public string Body { get; set; }
        public bool IsRead { get; set; }
        public DateTime SentAt { get; set; }
    }
}
