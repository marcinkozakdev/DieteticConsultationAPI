using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Exceptions;
using DieteticConsultationAPI.Models;
using Microsoft.EntityFrameworkCore;
using File = DieteticConsultationAPI.Entities.File;

namespace DieteticConsultationAPI.Services
{
    public class DietService : IDietService
    {
        private readonly DieteticConsultationDbContext _context;
        private readonly ILogger _logger;

        public DietService(DieteticConsultationDbContext context, ILogger<DietService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public int CreateDiet(CreateDietDto dto)
        {
            if (dto is null)
                throw new ArgumentNullException(nameof(dto));

            var diet = new Diet
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                CalorificValue = dto.CalorificValue,
                ProhibitedProducts = dto.ProhibitedProducts,
                RecommendedProducts = dto.RecommendedProducts,
                PatientId = dto.PatientId,

            };

            _context.Diets.Add(diet);
            _context.SaveChanges();

            return diet.Id;
        }

        public IEnumerable<DietDto> GetAllDiets()
        {
            var diets = _context
                .Diets
                .Include(d => d.Patient)
                .Include(d => d.Files)
                .ToList();

            var dietsDtos = diets.Select(d => new DietDto()
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                CalorificValue = d.CalorificValue,
                ProhibitedProducts = d.ProhibitedProducts,
                RecommendedProducts = d.RecommendedProducts,
                Files = d.Files.Select(Map).ToList(),
            });

            return dietsDtos;
        }

        public DietDto GetDiet(int id)
        {
            var diet = GetDietById(id);

            var dietDto = new DietDto()
            {
                Id = id,
                Name = diet.Name,
                Description = diet.Description,
                CalorificValue = diet.CalorificValue,
                ProhibitedProducts = diet.ProhibitedProducts,
                RecommendedProducts = diet.RecommendedProducts,
                Files = diet.Files.Select(Map).ToList(),
            };

            return dietDto;
        }

        public void UpdateDiet(UpdateDietDto dto, int id)
        {
            var diet = GetDietById(id);

            diet.Id = id;
            diet.Name = dto.Name;
            diet.Description = dto.Description;
            diet.CalorificValue = dto.CalorificValue;
            diet.ProhibitedProducts = dto.ProhibitedProducts;
            diet.RecommendedProducts = dto.RecommendedProducts;

            _context.SaveChanges();
        }

        public void DeleteDiet(int id)
        {
            _logger.LogWarning($"Diet with id: {id} DELETE action invoked");

            var diet = GetDietById(id);

            _context.Diets.Remove(diet);
            _context.SaveChanges();
        }

        private Diet GetDietById(int id)
        {
            var diet = _context
                .Diets
                .Include(d => d.Patient)
                .Include(d => d.Files)
                .FirstOrDefault(d => d.Id == id);

            if (diet == null)
                throw new NotFoundException("Diet not found");

            return diet;
        }

        private static FileDto? Map(File? file) =>
            file is null
            ? null
            : new FileDto
            {
                Id = file.Id,
                FileName = file.FileName,
                MimeType = file.MimeType,
                Attachment = file.Attachment,
            };
    }
}