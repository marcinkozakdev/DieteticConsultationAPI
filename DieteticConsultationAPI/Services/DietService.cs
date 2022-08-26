using AutoMapper;
using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Exceptions;
using DieteticConsultationAPI.Models;
using System.Xml.Linq;

namespace DieteticConsultationAPI.Services
{
    public interface IDietService
    {
        int Create(int patientId, CreateDietDto dto);

    }

    public class DietService : IDietService
    {
        private readonly DieteticConsultationDbContext _context;

        public int Create(int patientId, CreateDietDto dto)
        {
            var patient = _context.Patients.FirstOrDefault(p => p.Id == patientId);

            if (patient is null)
                throw new NotFoundException("Patient not found");

            var dietEntity = new Diet()
            {
                Name = dto.Name,
                Description = dto.Description,
                CalorificValue = dto.CalorificValue,
                ProhibitedProducts = dto.ProhibitedProducts,
                RecommendedProducts = dto.RecommendedProducts
            };

            _context.Diets.Add(dietEntity);
            _context.SaveChanges();

            return dietEntity.Id;
        }

       


    }
}
