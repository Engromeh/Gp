using Learning_Academy.Models;

namespace Learning_Academy.Repositories.Interfaces
{
    public interface ICertificateRepository
    {
        IEnumerable<Certificate> GetAllCertificate();
        Certificate GetCertificateById(int id);
        void AddCertificate(Certificate certificate);
        void UpdateCertificate(Certificate certificate);
        void DeleteCertificate(int id);
    }
}
