using System.ComponentModel.DataAnnotations;

namespace EventApi.Dtos
{
    public class FeedbackCreateDto
    {
        [Required]
        public int EventId { get; set; }

        public int? UserId { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public string? Comment { get; set; }
    }

    public class FeedbackUpdateDto
    {
        [Range(1, 5)]
        public int? Rating { get; set; }

        public string? Comment { get; set; }
    }

    public class FeedbackReadDto
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int? UserId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
