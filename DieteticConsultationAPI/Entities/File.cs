namespace DieteticConsultationAPI.Entities
{
    public class File
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public byte[] Attachment { get; set; }
        public DateTime Date { get; set; }
        public int? DietId { get; set; }
        public virtual Diet? Diet { get; set; }
    }
}
