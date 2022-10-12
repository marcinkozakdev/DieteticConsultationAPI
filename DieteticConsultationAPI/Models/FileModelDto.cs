namespace DieteticConsultationAPI.Entities
{
    public class FileModelDto
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public byte[] Attachment { get; set; }
        public DateTime Date { get; set; }
        public int? DietId { get; set; }
        public virtual Diet? Diet { get; set; }

        public static FileModelDto For(FileModel file)
            =>
            new()
            {
                DietId = file.DietId,
                FileName = file.FileName,
                FileType = file.FileType,
                Attachment = file.Attachment,
                Date = file.Date,
                Diet = file.Diet,
            };
    }
}
