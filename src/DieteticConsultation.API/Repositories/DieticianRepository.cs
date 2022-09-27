using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Exceptions;
using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Models.Pagination;
using DieteticConsultationAPI.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DieteticConsultationAPI.Repositories
{
    public class DieticianRepository : IDieticianRepository
    {
        private readonly DieteticConsultationDbContext _context;

        public DieticianRepository(DieteticConsultationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Dietician> GetAll() =>
            _context.Dieticians.Include(d => d.Patients);

        public Dietician? GetById(int? id) =>
            _context.Dieticians.FirstOrDefault(d => d.Id == id);

        public Dietician? AddOrUpdate(Dietician dietician)
        {
            // this have to be refactored, if you wanna create souch as this, first you have to check is element exist, if yes then fetch him, ovveride properites then use update method. If not exist then hus add it    
            _context.Dieticians.Add(dietician);
            _context.SaveChanges();

            if (dietician != null)
                _context.Dieticians.Update(dietician);
            _context.SaveChanges();

            return dietician;

            // todo test this 
            if (_context.Dieticians.FirstOrDefault(d => d.Id.Equals(dietician.Id)) is { } obj)
            {
                obj.Specialization = dietician.Specialization;
                // fill them all

                _context.Update(obj);
            }
            else
                _context.Add(dietician);
            
            _context.SaveChanges();
        }

        public void Delete(int? id)
        {
            Dietician? dietician = _context.Dieticians.FirstOrDefault(d => d.Id == id);
            _context.Dieticians.Remove(dietician);
            _context.SaveChanges();
        }
    }
}