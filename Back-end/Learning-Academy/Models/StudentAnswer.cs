using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_Academy.Models
{
    public class StudentAnswer
    {
        public int Id { get; set; }
        [ForeignKey("QuizAttempt")]
        public int SubmissionId { get; set; }
        public QuizSubmission? Submission { get; set; }

        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        public Question? Question { get; set; }
        [ForeignKey("Option")]
        public int SelectedOptionId { get; set; }
        public Option? SelectedOption { get; set; }
        public decimal PointsEarned { get; set; }
    }
}
