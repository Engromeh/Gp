using Learning_Academy.Models;
using Microsoft.AspNetCore.Mvc;

namespace Learning_Academy.Repositories.Interfaces
{
    public interface IInstructorRepostory
    {
        IEnumerable<Instructor> GetAllInstructors();
        Instructor GetByInstructorId(int id);
        void AddInstructor(Instructor instructor);
        void UpdateInstructor(Instructor instructor);
        void DeleteInstructor(int id);

    }
}
