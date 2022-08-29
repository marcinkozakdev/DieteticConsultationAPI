using Microsoft.EntityFrameworkCore;

namespace DieteticConsultationAPI.Entities
{
    public class DieteticConsultationDbContext : DbContext
    {
        private string _connectionString =
            "Server=(localdb)\\mssqllocaldb;Database=DieteticConsultationDb;Trusted_Connection=True;";

        public DbSet<Dietician> Dieticians { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Diet> Diets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dietician>(eb =>
            {
                eb.Property(d => d.FirstName).IsRequired();
                eb.Property(d => d.LastName).IsRequired();
                eb.Property(d => d.Specialization).IsRequired();
                eb.Property(d => d.ContactEmail).IsRequired();
                // Relacja: jeden - do wielu
                eb.HasMany(p => p.Patients)
                .WithOne(d => d.Dietician)
                .HasForeignKey(d => d.DieticianId);
            });

            modelBuilder.Entity<Patient>(eb =>
            {
                eb.Property(p => p.FirstName).IsRequired().HasMaxLength(25);
                eb.Property(p => p.LastName).IsRequired().HasMaxLength(25);
                eb.Property(p => p.ContactEmail).IsRequired();
                eb.Property(p => p.Height).IsRequired().HasPrecision(4, 1);
                eb.Property(p => p.Weight).IsRequired().HasPrecision(4, 1);
                // Relacja: jeden - jeden
                eb.HasOne(p => p.Diet)
                .WithOne(d => d.Patient)
                .HasForeignKey<Diet>(d => d.PatientId);
            });

            modelBuilder.Entity<User>(eb =>
            {
                eb.Property(u => u.Email).IsRequired();
            });

            modelBuilder.Entity<Role>(eb =>
            {
                eb.Property(u => u.Name).IsRequired();
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
