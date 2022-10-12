using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Extensions;

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
    public DietDto? Diet { get; set; }
    public int DieteticanId { get; set; }

    public static PatientDto For(Patient patient)
        =>
        patient is null
            ? null
            : new()
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

    public static async ValueTask<PatientDto?> BindAsync(HttpContext context)
    {
        var form = await context.Request.ReadFormAsync();

        form.TryGetValue(nameof(Id), out var id);
        form.TryGetValue(nameof(FirstName), out var firstName);
        form.TryGetValue(nameof(LastName), out var lastName);
        form.TryGetValue(nameof(ContactEmail), out var contactEmail);
        form.TryGetValue(nameof(ContactNumber), out var contactNumber);
        form.TryGetValue(nameof(Sex), out var sex);
        form.TryGetValue(nameof(Weight), out var weight);
        form.TryGetValue(nameof(Height), out var height);
        form.TryGetValue(nameof(Age), out var age);
        form.TryGetValue(nameof(Diet), out var diet);

        var result = new PatientDto
        {
            Id = int.TryParse(id, out var parsedId) ? parsedId : 0,
            FirstName = firstName,
            LastName = lastName,
            ContactEmail = contactEmail,
            ContactNumber = contactNumber,
            Sex = sex,
            Weight = decimal.TryParse(weight, out var parseWeight) ? parseWeight : 0,
            Height = decimal.TryParse(height, out var parseHeight) ? parseHeight : 0,
            Age = int.TryParse(age, out var parseAge) ? parseAge : 0,
            Diet = form.Deserialize<DietDto>(nameof(Diet)),
        };

        return await ValueTask.FromResult(result);
    }
}