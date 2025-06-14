namespace Learning_Academy.Models
{
    public class VideoProgress
    {
        public int Id { get; set; }
        public int VideoId { get; set; }
        public int StudentId { get; set; }
        public bool IsWatched { get; set; }
        public DateTime WatchedAt { get; set; }
        public Video? Video { get; set; }
        public Student? Student { get; set; }
    }
}
