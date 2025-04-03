using Learning_Academy.Models;
using Learning_Academy.Repositories.Interfaces;

namespace Learning_Academy.Repositories.Classes
{
    public class CertificateRepository : ICertificateRepository
    {
        private readonly LearningAcademyContext _context;
        public CertificateRepository(LearningAcademyContext context)
        {
            _context = context;
        }

        public IEnumerable<Certificate> GetAllCertificate()
        {
            return _context.Certificate;
        }

        public Certificate GetCertificateById(int id)
        {
            return _context.Certificate.SingleOrDefault(e => e.Id == id);
        }

        public void UpdateCertificate(Certificate certificate)
        {
            _context.Certificate.Update(certificate);
            _context.SaveChanges();
        }
        public void AddCertificate(Certificate certificate)
        {
            _context.Certificate.Add(certificate);
            _context.SaveChanges();
        }

        public void DeleteCertificate(int id)
        {
            var certificate = _context.Certificate.Find(id);
            if (certificate != null)
            {
                _context.Certificate.Remove(certificate);
                _context.SaveChanges();
            }
        }
    }
}
