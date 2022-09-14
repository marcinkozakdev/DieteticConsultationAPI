﻿
using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Exceptions;
using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Models.Pagination;
using DieteticConsultationAPI.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DieteticConsultationAPI.Repositories
{
    public class DieticianRepository : IDieticianRepository
    {
        private readonly DieteticConsultationDbContext _context;

        public DieticianRepository(DieteticConsultationDbContext context)
        {
            _context = context;
        }


        public ICollection<Dietician> GetAll() =>
            _context.Dieticians.ToList();

        public ICollection<Dietician> GetAllWithPatients() =>
          _context.Dieticians.Include(d => d.Patients).ToList();

        public Dietician? GetById(int? id) =>
            _context.Dieticians.FirstOrDefault(d => d.Id == id);

        public Dietician? AddOrUpdate(Dietician dietician)
        {
            _context.Dieticians.Add(dietician);
            _context.SaveChanges();

            if (dietician != null)
                _context.Dieticians.Update(dietician);
            _context.SaveChanges();

            return dietician;
        }

        public void Delete(int? id)
        {
            Dietician? dietician = _context.Dieticians.FirstOrDefault(d => d.Id == id);
            _context.Dieticians.Remove(dietician);
            _context.SaveChanges();

        }
    }
}