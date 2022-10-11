namespace DieteticConsultationAPI.Models
{
    public sealed record UpdateDieticianDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialization { get; set; }
        public string ContactEmail { get; set; }
        public string ContactNumber { get; set; }
    }
}
