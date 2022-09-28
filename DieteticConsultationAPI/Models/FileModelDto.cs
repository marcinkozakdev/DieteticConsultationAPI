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
    }
}
