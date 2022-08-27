using DieteticConsultationAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DieteticConsultationAPI.Models
{
    public class CreatePatientDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(25)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(25)]
        public string LastName { get; set; }
        [Required]
        public string ContactEmail { get; set; }
        public string ContactNumber { get; set; }
        public string Sex { get; set; }
        [Required]
        [Precision(4,1)]
        public decimal Weight { get; set; }
        [Required]
        [Precision(4, 1)]
        public decimal Height { get; set; }
        public int Age { get; set; }
        public int DieticianId { get; set; }
        


    }
}
