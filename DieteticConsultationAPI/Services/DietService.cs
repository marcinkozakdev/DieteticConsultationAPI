using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Exceptions;
using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Repositories.Abstractions;
using DieteticConsultationAPI.Services.Interfaces;
using FileModel = DieteticConsultationAPI.Entities.FileModel;

namespace DieteticConsultationAPI.Services
{
    public class DietService : IDietService
    {
        private readonly ILogger _logger;
        private readonly IDietRepository _dietRepository;

        public DietService(ILogger<DietService> logger, IDietRepository dietRepository)
        {
            _logger = logger;
            _dietRepository = dietRepository;
        }

        public int CreateDiet(CreateDietDto dto)
        {
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

            _dietRepository.AddOrUpdate(diet);

            return diet.Id;
        }

        public IEnumerable<DietDto> GetAllDiets()
        {
            var diets = _dietRepository.GetAllDietsWithPatientsAndFiles();

            if (diets is null)
                throw new NotFoundHttpException("The diet list is empty");

            var dietsDtos = diets.Select(d => new DietDto()
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                CalorificValue = d.CalorificValue,
                ProhibitedProducts = d.ProhibitedProducts,
                RecommendedProducts = d.RecommendedProducts,
                Files = d.Files?.Select(Map).ToList(),
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
                Files = diet.Files?.Select(Map).ToList(),
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

            _dietRepository.AddOrUpdate(diet);
        }

        public void DeleteDiet(int id)
        {
            _logger.LogWarning($"Diet with id: {id} DELETE action invoked");

            GetDietById(id);

            _dietRepository.Delete(id);
        }

        private Diet GetDietById(int id)
        {
            var diet = _dietRepository.GetDietWithPatientAndFiles(id);

            if (diet == null)
                throw new NotFoundHttpException("Diet not found");

            return diet;
        }

        private static FileModelDto? Map(FileModel? file) => file is null
            ? null
            : new FileModelDto
            {
                Id = file.Id,
                FileName = file.FileName,
                FileType = file.FileType,
                Attachment = file.Attachment
            };
    }
}