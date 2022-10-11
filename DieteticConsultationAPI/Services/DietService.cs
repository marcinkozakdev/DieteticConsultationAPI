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
        private readonly IDietRepository _dietRepository;

        public DietService(IDietRepository dietRepository) =>
            _dietRepository = dietRepository;


        public Task Create(DietDto dietDto) =>
            _dietRepository.AddOrUpdate(Diet.For(dietDto));

        public async Task<ICollection<DietDto>> GetAll() =>
             (await _dietRepository.GetAll())
            .Select(DietDto.For)
            .ToArray();

        public async Task<DietDto> GetById(int id)
        {
            var diet = DietDto.For(await _dietRepository.GetById(id));
            
            if (diet is null)
                CannotFindResourceException.For(id);

            return diet;
        }

        public Task Update(DietDto dietDto) => 
            _dietRepository.AddOrUpdate(Diet.For(dietDto));

        public async Task Delete(int id) =>
            await _dietRepository.Delete(id);
        
    }
}