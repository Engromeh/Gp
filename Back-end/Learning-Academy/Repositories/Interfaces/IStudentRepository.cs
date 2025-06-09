using Learning_Academy.DTO;
using Learning_Academy.Models;

namespace Learning_Academy.Repositories.Interfaces
{
    public interface IStudentRepository
    {
        
        Task<IEnumerable<StudentDto>> GetAllStudentsAsync();
       Task< StudentDto> GetByStudentId(int id);
        void AddStudent(Student student);

        void UpdateStudent(Task<StudentDto> student);
        void DeleteStudent(int id);
        Student GetByUserId(string userId);
        
    }
}
