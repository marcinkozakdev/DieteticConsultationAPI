using DieteticConsultationAPI.Entities;
namespace DieteticConsultationAPI.Models;

public class DieticianDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Specialization { get; set; }
    public string ContactEmail { get; set; }
    public string ContactNumber { get; set; }
    public List<PatientDto> Patients { get; set; }

    public static DieticianDto For(Dietician dietician)
        => new()
        {
            Id = dietician.Id,
            FirstName = dietician.FirstName,
            LastName = dietician.LastName,
            Specialization = dietician.Specialization,
            ContactEmail = dietician.ContactEmail,
            ContactNumber = dietician.ContactNumber,
            Patients = dietician.Patients.Select(PatientDto.For).ToList()
        };
}