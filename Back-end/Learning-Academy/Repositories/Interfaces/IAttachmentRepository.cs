using Learning_Academy.Models;

namespace Learning_Academy.Repositories.Interfaces
{
    public interface IAttachmentRepository
    {
        IEnumerable<Attachment> GetAllAttach();
        Attachment GetAttachById(int id);
        void AddAttach(Attachment attachment);
        void UpdateAttach(Attachment attachment);
        void DeleteAttach(int id);
    }
}
