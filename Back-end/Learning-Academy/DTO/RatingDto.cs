using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_Academy.DTO
{
    public class RatingDto
    {
        public int Id { get; set; }
        public int RatingValue { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public string StudentName { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
    }
    public class CreateRatingDto
    {
        public int RatingValue { get; set; }
        public string Comment { get; set; }

    }
    public class CourseWithRatingsDto
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public double AverageRating { get; set; }
        public int TotalRatings { get; set; }
        public List<RatingDto> Ratings { get; set; }
    }
}
