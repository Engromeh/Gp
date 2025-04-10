using Learning_Academy.DTO;

namespace Learning_Academy.Services
{
    public interface IVideoService
    {
        Task<VideoResponseDto> UploadVideoAsync(VideoUploadDto videoUploadDto);
        Task<VideoResponseDto> GetVideoAsync(int id);
        Task<IEnumerable<VideoResponseDto>> GetVideosByCourseAsync(int courseId);
        Task<bool> DeleteVideoAsync(int id);
    }
}
