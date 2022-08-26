namespace DieteticConsultationAPI.Models
{
    public class UpdateDietDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int CalorificValue { get; set; }
        public string? ProhibitedProducts { get; set; }
        public string? RecommendedProducts { get; set; }


        public int PatientId { get; set; }
    }
}
