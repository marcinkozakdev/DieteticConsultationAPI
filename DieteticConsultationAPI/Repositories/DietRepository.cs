using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DieteticConsultationAPI.Repositories
{
    public class DietRepository : IDietRepository
    {
        private readonly DieteticConsultationDbContext _context;

        public DietRepository(DieteticConsultationDbContext context)
        {
            _context = context;
        }
        public Diet? AddOrUpdate(Diet? diet)
        {
            _context.Diets.Add(diet);
            _context.SaveChanges();

            if (diet != null)
                _context.Diets.Update(diet);
            _context.SaveChanges();

            return diet;
        }

        public Diet? Delete(int? id)
        {
            Diet? diet = _context
                .Diets
                .FirstOrDefault(d => d.Id == id);

            _context.Diets.Remove(diet);
            _context.SaveChanges();

            return diet;
        }

        public IEnumerable<Diet>? GetAll() => _context
            .Diets
            .Include(d => d.Patient)
            .Include(d => d.Files)?
            .ToList();


        public Diet? GetById(int? id) => _context
            .Diets
            .Include(d => d.Patient)
            .Include(d => d.Files)?
            .FirstOrDefault(d => d.Id == id);
    }
}
