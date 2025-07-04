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
        public IEnumerable<Video> GetAllVideos()
        {
            return _context.Videos;
          
        }

        public Video GetVideoById(int id)
        {
            return _context.Videos.SingleOrDefault(e => e.Id == id);
        }

        public void UpdateVideo(Video video)
        {
            _context.Videos.Update(video);
            _context.SaveChanges();
        }
        public void AddVideo(Video video)
        {

            _context.Videos.Add(video);
            _context.SaveChanges();
        }


        public void DeleteVideo(int id)
        {
            var video = _context.Videos.Find(id);
            if (video != null)
            {
                // نحذف ملف الفيديو من السيرفر
                var videoPath = Path.Combine("wwwroot", "videos", video.VideoPath);
                if (File.Exists(videoPath))
                {
                    File.Delete(videoPath);
                }

                _context.Videos.Remove(video);
                _context.SaveChanges();
            }
        }
    
    }
}
