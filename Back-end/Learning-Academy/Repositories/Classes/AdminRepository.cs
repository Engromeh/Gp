using Learning_Academy.Models;
using Microsoft.EntityFrameworkCore;
using Learning_Academy.DTO;
using Learning_Academy.Repositories.Interfaces;

namespace Learning_Academy.Repositories.Classes
{
    public class AdminRepository : IAdminRepository
    {

        private readonly LearningAcademyContext _context;
        public AdminRepository(LearningAcademyContext context)
        {
            _context = context;
        }

          public IEnumerable<Admin> GetAllAdmins()
          {
            return _context.Admins;
          }

        public Admin GetByAdminId(int id)
        {
            return _context.Admins.SingleOrDefault(e => e.Id == id);
        }

        public void UpdateAdmin(Admin admin)
        {
            _context.Admins.Update(admin);
            _context.SaveChanges();
        }
        public void AddAdmin(Admin admin)
        {
            _context.Admins.Add(admin);
            _context.SaveChanges();
        }

        public void DeleteAdmin(int id)
        {
            var admin = _context.Admins.Find(id);
            if (admin != null)
            {
                _context.Admins.Remove(admin);
                _context.SaveChanges();
            }
        }

        public async Task<int> GetTotalCoursesCountAsync()
        {
            return await _context.Courses.CountAsync();
        }

        public async Task<int> GetTotalStudentsCountAsync()
        {
            return await _context.Students.CountAsync();
        }

        public async Task<int> GetTotalQuizzesCountAsync()
        {
            return await _context.Quizzes.CountAsync();
        }
    }
}
