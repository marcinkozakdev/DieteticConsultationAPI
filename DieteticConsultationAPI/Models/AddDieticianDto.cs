using DieteticConsultationAPI.Entities;
using System.ComponentModel.DataAnnotations;

namespace DieteticConsultationAPI.Models
{
    public class AddDieticianDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialization { get; set; }
        public string ContactEmail { get; set; }
        public string ContactNumber { get; set; }
        public virtual Dietician Dietician { get; set; }
    }
}
