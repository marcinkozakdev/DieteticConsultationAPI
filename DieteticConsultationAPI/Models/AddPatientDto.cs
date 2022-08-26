namespace DieteticConsultationAPI.Models
{
    public class AddPatientDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactNumber { get; set; }
        public string Sex { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public int Age { get; set; }
        public DietDto? Diet { get; set; }
    }
}
