using DieteticConsultationAPI.Entities;
using System.ComponentModel.DataAnnotations;

namespace DieteticConsultationAPI.Models
{
    public class CreateDieticianDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialization { get; set; }
        [EmailAddress]
        public string ContactEmail { get; set; }
        [Phone]
        public string ContactNumber { get; set; }
        

    }
}
