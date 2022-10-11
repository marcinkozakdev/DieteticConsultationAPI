using DieteticConsultationAPI.Entities;
namespace DieteticConsultationAPI.Models;

public sealed record PatientDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ContactEmail { get; set; }
    public string ContactNumber { get; set; }
    public string Sex { get; set; }
    public decimal Weight { get; set; }
    public decimal Height { get; set; }
    public int Age { get; set; }
    public DietDto Diet { get; set; }
    public int DieteticanId { get; set; }

    public static PatientDto For(Patient patient) =>
        new()
        {
            Id = patient.Id,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            ContactEmail = patient.ContactEmail,
            ContactNumber = patient.ContactNumber,
            Sex = patient.Sex,
            Weight = patient.Weight,
            Height = patient.Height,
            Age = patient.Age,
            Diet = DietDto.For(patient.Diet)
        };
}