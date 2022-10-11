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

        public async Task<Diet> AddOrUpdate(Diet diet)
        {
            if (await _context.Diets.FirstOrDefaultAsync(d => d.Id.Equals(diet.Id)) is { } obj)
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
                await _context.Diets.AddAsync(diet);

            await _context.SaveChangesAsync();

            return diet;
        }

        public async Task Delete(int id)
        {
            if (await _context.Diets.FirstOrDefaultAsync(d => d.Id == id) is { } diet)
            {
                _context.Diets.Remove(diet);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ICollection<Diet>> GetAll() =>
            await _context
                .Diets
                .Include(d => d.Patient)
                .Include(d => d.Files)?
                .ToListAsync();

        public async Task<Diet> GetById(int id) =>
            await _context
                .Diets
                .Include(d => d.Patient)
                .Include(d => d.Files)?
                .FirstOrDefaultAsync(d => d.Id == id);
    }
}
