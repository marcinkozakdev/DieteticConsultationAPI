using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Exceptions;
using DieteticConsultationAPI.Extensions;
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
        public async Task<int> AddOrUpdateDietician(DieticianDto dto, int id)
        {
            var dietician = await _dieticianRepository.GetById(id);

            if (dietician is null)
            {
                dietician = new Dietician()
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Specialization = dto.Specialization,
                    ContactEmail = dto.ContactEmail,
                    ContactNumber = dto.ContactNumber
                };
            }

            else
            {
                dietician.FirstName = dto.FirstName;
                dietician.LastName = dto.LastName;
                dietician.Specialization = dto.Specialization;
                dietician.ContactEmail = dto.ContactEmail;
                dietician.ContactNumber = dto.ContactNumber;
            }

            await _dieticianRepository.AddOrUpdate(dietician);

            return dietician.Id;
        }

        public async Task<IEnumerable<DieticianDto>> GetAllDieticians()
        {
            var dieticians = await _dieticianRepository.GetAll();

            if (dieticians.IsEmpty())
                NotFoundHttpException.For("The diet list is empty");

            var dieticiansDtos = dieticians!.Select(DieticianDto.For).ToList();

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
                Patients = dietician.Patients.Select(PatientDto.For).ToList()
            };

            return dieticianDto;
        }

        // use just one method like create Dietetican, when it is exist then it  will update if not then it will create new 

        // public async Task UpdateDietician(UpdateDieticianDto dto, int id)
        // {
        //     var dietician = await _dieticianRepository.GetById(id);
        //
        //     dietician.FirstName = dto.FirstName;
        //     dietician.LastName = dto.LastName;
        //     dietician.Specialization = dto.Specialization;
        //     dietician.ContactEmail = dto.ContactEmail;
        //     dietician.ContactNumber = dto.ContactNumber;
        //
        //     await _dieticianRepository.AddOrUpdate(dietician);
        // }

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
    }
}