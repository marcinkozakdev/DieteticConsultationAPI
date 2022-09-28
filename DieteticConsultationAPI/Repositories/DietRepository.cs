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
            if (_context.Diets.FirstOrDefault(d => d.Id.Equals(diet.Id)) is { } obj)
            {
                obj.Description = diet?.Description;
                obj.CalorificValue = diet.CalorificValue;
                obj.RecommendedProducts = diet.RecommendedProducts;
                obj.ProhibitedProducts = diet.ProhibitedProducts;
                obj.Files = diet.Files;
                obj.Name = diet.Name;

                _context.Update(obj);
            }

            else
                _context.Add(diet);

            _context.SaveChanges();

            return diet;
        }

        public void Delete(int? id)
        {
            Diet? diet = _context
                .Diets
                .FirstOrDefault(d => d.Id == id);

            _context.Diets.Remove(diet);
            _context.SaveChanges();
        }

        public IEnumerable<Diet>? GetAllDietsWithPatientsAndFiles() => _context
            .Diets
            .Include(d => d.Patient)
            .Include(d => d.Files)?
            .ToList();

        public Diet? GetDietWithPatientAndFiles(int? id) => _context
            .Diets
            .Include(d => d.Patient)
            .Include(d => d.Files)?
            .FirstOrDefault(d => d.Id == id);
    }
}
