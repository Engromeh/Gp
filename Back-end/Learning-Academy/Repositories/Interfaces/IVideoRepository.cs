using Learning_Academy.Models;

namespace Learning_Academy.Repositories.Interfaces
{
    public interface IVideoRepository
    {
        IEnumerable<Video> GetAllVideos();
        Video GetVideoById(int id);
        //Task<Video> UploadVideo(IFormFile file, int courseId);
        void AddVideo(Video video);
        void UpdateVideo(Video video);
        void DeleteVideo(int id);

        //upload video
        Task<Video> AddVideoAsync(Video video);
        Task<Video> GetVideoByIdAsync(int id);
        Task<bool> DeleteVideoAsync(int id);
        Task<IEnumerable<Video>> GetVideosByCourseIdAsync(int courseId);
    }
    

}
