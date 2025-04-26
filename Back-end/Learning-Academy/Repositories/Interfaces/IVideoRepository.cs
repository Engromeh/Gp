using Learning_Academy.Models;
using Microsoft.EntityFrameworkCore;

namespace Learning_Academy.Repositories.Interfaces
{
    public interface IVideoRepository
    {
        IEnumerable<Video> GetAllVideos();
        Video GetVideoById(int id);
        void AddVideo(Video video);
        void UpdateVideo(Video video);
        void DeleteVideo(int id);

    }
}
