using Learning_Academy.Models;

namespace Learning_Academy.Repositories.Interfaces
{
    public interface IMassageRepository
    {
        IEnumerable<Massage> GetAllMassage();
        Massage GetMassageById(int id);
        void AddMassage(Massage massage);
        void DeleteMassage(int id);
        void UpdateMassage(Massage massage);
    }
}
