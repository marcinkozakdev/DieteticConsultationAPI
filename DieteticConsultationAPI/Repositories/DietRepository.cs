using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Exceptions;
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

        public async Task AddOrUpdate(Diet diet)
        {
            if (await _context.Diets.FirstOrDefaultAsync(d => d.Id == diet.Id) is { } obj)
            {
                obj.Description = diet?.Description;
                obj.CalorificValue = diet.CalorificValue;
                obj.RecommendedProducts = diet.RecommendedProducts;
                obj.ProhibitedProducts = diet.ProhibitedProducts;
                obj.Files = diet.Files;
                obj.Name = diet.Name;

                _context.Update(obj);
            }

            else if (diet.Id is 0)
                await _context.Diets.AddAsync(diet);

            else
                CannotFindResourceException.For(diet.Id);

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            if (await _context.Diets.FirstOrDefaultAsync(d => d.Id == id) is { } diet)
            {
                _context.Diets.Remove(diet);
                await _context.SaveChangesAsync();
            }
            else
                CannotFindResourceException.For(id);
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
