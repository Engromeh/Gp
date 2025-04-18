﻿using Learning_Academy.Models;
using Learning_Academy.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Learning_Academy.Repositories.Classes
{
    public class VideoRepository : IVideoRepository
    {
        private readonly LearningAcademyContext _context;
        public VideoRepository(LearningAcademyContext context)
        {
            _context = context;
        }
        

        //Upload video
        public async Task<Video> AddVideoAsync(Video video)
        {
            await _context.Videos.AddAsync(video);
            await _context.SaveChangesAsync();
            return video;
        }

        public async Task<Video> GetVideoByIdAsync(int id)
        {
            return await _context.Videos.FindAsync(id);
        }

        async Task<bool> IVideoRepository.DeleteVideoAsync(int id)
        {
            var video = await _context.Videos.FindAsync(id);
            if (video == null)
                return false;

            _context.Videos.Remove(video);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Video>> GetVideosByCourseIdAsync(int courseId)
        {
            return await _context.Videos
                .Where(v => v.CourseId == courseId)
                .ToListAsync();
        }

    }
}
