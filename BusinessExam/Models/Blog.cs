using BusinessExam.Models.Common;

namespace BusinessExam.Models
{
    public class Blog:BaseEntity
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }

    }
}
