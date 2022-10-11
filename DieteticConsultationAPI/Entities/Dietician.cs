using DieteticConsultationAPI.Models;
namespace DieteticConsultationAPI.Entities;

public class Dietician
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Specialization { get; set; }
    public string ContactEmail { get; set; }
    public string ContactNumber { get; set; }
    public ICollection<Patient> Patients { get; set; }
    public static Dietician For(DieticianDto dieticianDto) =>
        new()
        {
            Id = dieticianDto.Id,
            FirstName = dieticianDto.FirstName,
            LastName = dieticianDto.LastName,
            Specialization = dieticianDto.Specialization,
            ContactEmail = dieticianDto.ContactEmail,
            ContactNumber = dieticianDto.ContactNumber,
            //Patients= dieticianDto.Patients.Select(Patient.For).ToList()
        };
}