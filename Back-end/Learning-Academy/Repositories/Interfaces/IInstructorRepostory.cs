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

    }
}
