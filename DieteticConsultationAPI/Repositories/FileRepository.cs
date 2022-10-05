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

        public async Task Delete(int? id)
        {
            FileModel file = _context
                .Files
                .FirstOrDefault(d => d.Id == id);

            _context.Files.Remove(file);
            await _context.SaveChangesAsync();
        }

        public async Task<FileModel> GetById(int? id) => await Task.FromResult(_context
            .Files
            .FirstOrDefault(d => d.Id == id));


        public async Task Upload(FileModel file)
        {
            _context.Files.Add(file);
            await _context.SaveChangesAsync();
        }
    }
}
        