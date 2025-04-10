using Learning_Academy.Models;
using Learning_Academy.Repositories.Interfaces;

namespace Learning_Academy.Repositories.Classes
{
    public class AttachmentRepository : IAttachmentRepository
    {
        private readonly LearningAcademyContext _context;
        public AttachmentRepository(LearningAcademyContext context)
        {
            _context = context;
        }

        public IEnumerable<Attachment> GetAllAttach()
        {
            return _context.Attachments;
        }

        public Attachment GetAttachById(int id)
        {
            return _context.Attachments.SingleOrDefault(e => e.Id == id);
        }

        public void UpdateAttach(Attachment attachment)
        {
            _context.Attachments.Update(attachment);
            _context.SaveChanges();
        }
        public void AddAttach(Attachment attachment)
        {
            _context.Attachments.Add(attachment);
            _context.SaveChanges();
        }

        public void DeleteAttach(int id)
        {
            var attachment = _context.Attachments.Find(id);
            if (attachment != null)
            {
                _context.Attachments.Remove(attachment);
                _context.SaveChanges();
            }
        }
    }
}
