namespace Learning_Academy.DTO
{
    public class QuizCreateRequest
    {
        public string Title { get; set; }
        public int CourseId { get; set; }
        public DateTime? DueDate { get; set; }
        public int TimeLimitMinutes { get; set; } = 0;
    }

    public class QuizResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int CourseId { get; set; }
        public DateTime? DueDate { get; set; }
        public int TimeLimitMinutes { get; set; }
    }
    

    public class QuestionRequest
    {
        public string Text { get; set; }
        
        public int Points { get; set; }
       
        public List<AnswerOptionRequest> AnswerOptions { get; set; }
    }

    public class AnswerOptionRequest
    {
        
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }

    public class QuizSubmissionRequest
    {    
        public int QuizId { get; set; }  
        public List<StudentAnswerRequest> Answers { get; set; }
    }

    public class StudentAnswerRequest
    {
       
        public int QuestionId { get; set; }
        public int SelectedOptionId { get; set; }
    }

    public class QuizSubmissionResponse
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public int StudentId { get; set; }
        public DateTime SubmissionTime { get; set; }
        public decimal Score { get; set; }
        public decimal TotalPossiblePoints { get; set; }
        
        public List<StudentAnswerResponse> Answers { get; set; }
    }

    public class StudentAnswerResponse
    {
        public int QuestionId { get; set; }
        public int SelectedOptionId { get; set; }
        public decimal PointsEarned { get; set; }
    }
}
