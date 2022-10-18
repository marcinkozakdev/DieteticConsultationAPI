using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Exceptions;
using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Repositories.Abstractions;
using DieteticConsultationAPI.Services.Interfaces;


namespace DieteticConsultationAPI.Services
{
    public class DieticianService : IDieticianService
    {
        private readonly IDieticianRepository _dieticianRepository;
        public DieticianService(IDieticianRepository dieticianRepository) =>
            _dieticianRepository = dieticianRepository;

        public Task Create(DieticianDto dieticianDto) =>
            _dieticianRepository.AddOrUpdate(Dietician.For(dieticianDto));

        public async Task<ICollection<DieticianDto>> GetAll() =>
            (await _dieticianRepository.GetAll())
            .Select(DieticianDto.For)
            .ToArray();

        public async Task<DieticianDto> GetById(int id)
        {
            var dietetican = DieticianDto.For(await _dieticianRepository.GetById(id));

            if (dietetican is null)
                CannotFindResourceException.For(id);

            return dietetican;
        }

        public Task Update(DieticianDto dieticianDto)
        {
            if (dieticianDto.Id is 0)
                IdNotProvidedException.For();

            return _dieticianRepository.AddOrUpdate(Dietician.For(dieticianDto));
        }

        public async Task Delete(int id) =>
            await _dieticianRepository.Delete(id);
    }

}