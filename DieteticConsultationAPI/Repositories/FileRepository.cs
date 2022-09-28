using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Repositories.Abstractions;

namespace DieteticConsultationAPI.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly DieteticConsultationDbContext _context;

        public FileRepository(DieteticConsultationDbContext context)
        {
            _context = context;
        }

        public void Delete(int? id)
        {
            FileModel? file = _context
                .Files
                .FirstOrDefault(d => d.Id == id);

            _context.Files.Remove(file);
            _context.SaveChanges();
        }

        public FileModel? GetById(int? id) => _context
            .Files
            .FirstOrDefault(d => d.Id == id);


        public void Upload(FileModel file)
        {
            _context.Files.Add(file);
            _context.SaveChanges();
        }
    }
}
