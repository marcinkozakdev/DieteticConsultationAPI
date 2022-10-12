using System.Net.Mail;

namespace DieteticConsultationAPI.Entities
{
    public class FileModel
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public byte[] Attachment { get; set; }
        public DateTime Date { get; set; }
        public int? DietId { get; set; }
        public virtual Diet? Diet { get; set; }

        public static FileModel For(FileModelDto fileDto)
            =>
            new()
            {
                DietId = fileDto.DietId,
                FileName = fileDto.FileName,
                FileType = fileDto.FileType,
                Attachment = fileDto.Attachment,
                Date = fileDto.Date,
                Diet = fileDto.Diet,
            };
    }
}
