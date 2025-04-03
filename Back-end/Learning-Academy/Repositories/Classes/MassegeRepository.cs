using Learning_Academy.Models;
using Learning_Academy.Repositories.Interfaces;
using System.Linq;

namespace Learning_Academy.Repositories.Classes
{
    public class MassegeRepository : IMassageRepository
    {
        private readonly LearningAcademyContext _context;
        public MassegeRepository(LearningAcademyContext context)
        {
            _context = context;
        }

        public IEnumerable<Massage> GetAllMassage()
        {
            return _context.Massages;
        }

        public Massage GetMassageById(int id)
        {
            return _context.Massages.SingleOrDefault(e => e.Id == id);
        }

        public void UpdateMassage(Massage massage)
        {
            _context.Massages.Update(massage);
            _context.SaveChanges();
        }
        public void AddMassage(Massage massage)
        {
            _context.Massages.Add(massage);
            _context.SaveChanges();
        }

        public void DeleteMassage(int id)
        {
            var massege = _context.Massages.Find(id);
            if (massege != null)
            {
                _context.Massages.Remove(massege);
                _context.SaveChanges();
            }
        }
    }
}
