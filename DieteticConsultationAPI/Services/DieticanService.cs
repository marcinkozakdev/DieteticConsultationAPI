using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Exceptions;
using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Repositories.Abstractions;
using DieteticConsultationAPI.Services.Interfaces;


namespace DieteticConsultationAPI.Services
{
    public class DieticianService : IDieticianService
    {
        private readonly ILogger _logger;
        private readonly IDieticianRepository _dieticianRepository;

        public DieticianService(ILogger<DieticianService> logger, IDieticianRepository dieticianRepository)
        {
            _logger = logger;
            _dieticianRepository = dieticianRepository;
        }
        public async Task<int> CreateDietician(CreateDieticianDto dto)
        {
            var dietician = new Dietician()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Specialization = dto.Specialization,
                ContactEmail = dto.ContactEmail,
                ContactNumber = dto.ContactNumber
            };

            await _dieticianRepository.AddOrUpdate(dietician);

            return dietician.Id;
        }

        public async Task<IEnumerable<DieticianDto>> GetAllDieticians()
        {
            var dieticians = await _dieticianRepository.GetAll();

            if (dieticians is null)
                NotFoundHttpException.For("The diet list is empty");

            var dieticiansDtos = dieticians.Select(d => new DieticianDto()
            {
                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName,
                Specialization = d.Specialization,
                ContactEmail = d.ContactEmail,
                ContactNumber = d.ContactNumber,
                Patients = d.Patients.Select(Map).ToList(),
            }).ToList();

            return dieticiansDtos;
        }

        public async Task<DieticianDto> GetDietician(int id)
        {
            var dietician = await GetDieticianById(id);

            var dieticianDto = new DieticianDto()
            {
                Id = id,
                FirstName = dietician.FirstName,
                LastName = dietician.LastName,
                Specialization = dietician.Specialization,
                ContactEmail = dietician.ContactEmail,
                ContactNumber = dietician.ContactNumber,
                Patients = dietician.Patients.Select(Map).ToList()
            };

            return dieticianDto;
        }

        public async Task UpdateDietician(UpdateDieticianDto dto, int id)
        {
            var dietician = await GetDieticianById(id);

            dietician.FirstName = dto.FirstName;
            dietician.LastName = dto.LastName;
            dietician.Specialization = dto.Specialization;
            dietician.ContactEmail = dto.ContactEmail;
            dietician.ContactNumber = dto.ContactNumber;

            await _dieticianRepository.AddOrUpdate(dietician);
        }

        public async Task DeleteDietician(int id)
        {
            _logger.LogWarning($"Dietician with id: {id} DELETE action invoked");

            var dietician = await GetDieticianById(id);

            await _dieticianRepository.Delete(dietician.Id);
        }

        public async Task<Dietician> GetDieticianById(int id)
        {
            var dietician = await _dieticianRepository.GetById(id);

            if (dietician is null)
                NotFoundHttpException.For("Dietician not found");

            return dietician;
        }

        private PatientDto Map(Patient patient) =>
            new PatientDto
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                ContactEmail = patient.ContactEmail,
                ContactNumber = patient.ContactNumber,
                Sex = patient.Sex,
                Weight = patient.Weight,
                Height = patient.Height,
                Age = patient.Age,
                Diet = Map(patient.Diet)
            };

        private static DietDto Map(Diet diet) =>
            diet is null
            ? null
            : new DietDto
            {
                Id = diet.Id,
                Name = diet.Name,
                Description = diet.Description,
                CalorificValue = diet.CalorificValue,
                ProhibitedProducts = diet.ProhibitedProducts,
                RecommendedProducts = diet.RecommendedProducts
            };
    }
}
