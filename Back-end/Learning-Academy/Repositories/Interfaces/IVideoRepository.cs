using Learning_Academy.Models;

namespace Learning_Academy.Repositories.Interfaces
{
    public interface IVideoRepository
    {
        
       
        //upload video
        Task<Video> AddVideoAsync(Video video);
        Task<Video> GetVideoByIdAsync(int id);
        Task<bool> DeleteVideoAsync(int id);
        Task<IEnumerable<Video>> GetVideosByCourseIdAsync(int courseId);
    }
    

}
