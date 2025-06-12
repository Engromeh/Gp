using Learning_Academy.DTO;
using Learning_Academy.Models;
using Microsoft.AspNetCore.Mvc;

namespace Learning_Academy.Repositories.Interfaces
{
    public interface IInstructorRepostory
    {
        IEnumerable<InstructorDto> GetAllInstructors();
        InstructorDto GetByInstructorId(int id);
        void AddInstructor(Instructor instructor);
        void UpdateInstructor(InstructorDto instructor);
        void DeleteInstructor(int id);
        Task<int> CountInstructorQuizzesAsync(string userId);
        Task<int> CountInstructorCoursesAync(string userId);
        Task<int>CountInstructorStudentsAsync(string userId);
        Task<List<StudentThatEnrollmentWtithInstructorDto>> GetStudentsWithTheirCoursesAsync(string instructorUserId);

    }
}
