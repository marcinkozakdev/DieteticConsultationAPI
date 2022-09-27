namespace DieteticConsultationAPI.Models
{
    public class CreateDieticianDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialization { get; set; }
        public string ContactEmail { get; set; }
        public string ContactNumber { get; set; }
        public List<CreatePatientDto> Patients { get; set; }
    }
}
