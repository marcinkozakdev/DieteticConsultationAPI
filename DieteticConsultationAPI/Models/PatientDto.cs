using DieteticConsultationAPI.Entities;
using System.ComponentModel.DataAnnotations;

namespace DieteticConsultationAPI.Models
{
    public class PatientDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress]
        public string ContactEmail { get; set; }
        [Phone]
        public string ContactNumber { get; set; }
        public string Sex { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public int Age { get; set; }
        public virtual DietDto? Diet { get; set; }
    }
}
