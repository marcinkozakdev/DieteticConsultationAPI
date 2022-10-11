using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Extensions;
namespace DieteticConsultationAPI.Models;

public class DieticianDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Specialization { get; set; }
    public string ContactEmail { get; set; }
    public string ContactNumber { get; set; }
    public ICollection<PatientDto> Patients { get; set; }

    public static DieticianDto For(Dietician dietician)
        =>
            dietician is null
                ? null
                : new()
                {
                    Id = dietician.Id,
                    FirstName = dietician.FirstName,
                    LastName = dietician.LastName,
                    Specialization = dietician.Specialization,
                    ContactEmail = dietician.ContactEmail,
                    ContactNumber = dietician.ContactNumber,
                    Patients = dietician.Patients.Select(PatientDto.For).ToList()
                };
    public static async ValueTask<DieticianDto?> BindAsync(HttpContext context)
    {
        var form = await context.Request.ReadFormAsync();

        form.TryGetValue(nameof(Id), out var id);
        form.TryGetValue(nameof(Specialization), out var specialization);
        form.TryGetValue(nameof(ContactEmail), out var contactEmail);
        form.TryGetValue(nameof(FirstName), out var firstName);
        form.TryGetValue(nameof(ContactNumber), out var contactNumber);
        form.TryGetValue(nameof(LastName), out var lastName);

        //var a = form.Deserialize<ICollection<PatientDto>>(nameof(Patients));

        var result = new DieticianDto
        {
            Id = int.TryParse(id, out var parsedId) ? parsedId : 0,
            Patients = form.Deserialize<ICollection<PatientDto>>(nameof(Patients)),
            Specialization = specialization,
            ContactEmail = contactEmail,
            ContactNumber = contactNumber,
            FirstName = firstName,
            LastName = lastName
        };

        return await ValueTask.FromResult(result);
    }
}