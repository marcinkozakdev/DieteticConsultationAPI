using System.ComponentModel.DataAnnotations;

namespace DieteticConsultationAPI.Models
{
    public class UpdateDieticianDto
    {
   
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialization { get; set; }
        [EmailAddress]
        public string ContactEmail { get; set; }
        [Phone]
        public string ContactNumber { get; set; }

    }
}
