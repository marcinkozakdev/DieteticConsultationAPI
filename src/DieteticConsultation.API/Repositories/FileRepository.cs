using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Repositories.Abstractions;

namespace DieteticConsultationAPI.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly DieteticConsultationDbContext _context;
        public FileRepository(DieteticConsultationDbContext context) => _context = context;

        public void Delete(int? id)
        {
            var file = _context
                .Files
                .FirstOrDefault(d => d.Id == id);

            _context.Files.Remove(file);
            _context.SaveChanges();
        }

        public FileModel? GetById(int? id) => _context
            .Files
            .FirstOrDefault(d => d.Id == id);


        // why this parameter is nullable, throw exception in service if null
        public void Upload(FileModel? file)
        {
            _context.Files.Add(file);
            _context.SaveChanges();
        }
    }
}
