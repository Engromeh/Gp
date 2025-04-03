using Learning_Academy.Models;

namespace Learning_Academy.Repositories.Interfaces
{
    public interface IAdminRepository
    {
        IEnumerable<Admin> GetAllAdmins();

        Admin GetByAdminId(int id);

        void AddAdmin(Admin admin);
        void UpdateAdmin(Admin admin);
        void DeleteAdmin(int id);
    }
}
