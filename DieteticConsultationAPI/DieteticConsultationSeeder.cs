using DieteticConsultationAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace DieteticConsultationAPI
{
    public class DieteticConsultationSeeder
    {
        private readonly DieteticConsultationDbContext _dbContext;

        public DieteticConsultationSeeder(DieteticConsultationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                var pendingMigrations = _dbContext.Database.GetPendingMigrations();

                if(pendingMigrations !=null && pendingMigrations.Any())
                {
                    _dbContext.Database.Migrate();
                }
                
                if (!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }

                if (!_dbContext.Dieticians.Any())
                {
                    var dieticans = GetDieticans();
                    _dbContext.Dieticians.AddRange(dieticans);
                    _dbContext.SaveChanges();
                }

                if (!_dbContext.Patients.Any())
                {
                    var patients = GetPatients();
                    _dbContext.Patients.AddRange(patients);
                    _dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = "Admin"
                },

                new Role()
                {
                    Name = "Dietician"
                },

                new Role()
                {
                    Name = "Patient"
                },
            };

            return roles;
        }

        private IEnumerable<Dietician> GetDieticans()
        {
            var dieticans = new List<Dietician>()
            {
                new Dietician()
                {
                    FirstName = "Dominika",
                    LastName = "Kozak",
                    Specialization = "",
                    ContactEmail = "kozakdietetyk@test.com",
                    ContactNumber = "777888999",
                    Patients = new List<Patient>(),
                    Id = 1
                }
            };

            return dieticans;
        }

        private IEnumerable<Patient> GetPatients()
        {
            var patients = new List<Patient>()
            {
                new Patient()
                {
                    FirstName = "Marcin",
                    LastName = "Kozak",
                    ContactEmail = "marcinkozak@test.com",
                    ContactNumber = "999888777",
                    Sex = "Male",
                    Weight = 60,
                    Height = 168,
                    Age = 28,
                    DieticianId = 1
                }
            };

            return patients;
        }
    }
}
