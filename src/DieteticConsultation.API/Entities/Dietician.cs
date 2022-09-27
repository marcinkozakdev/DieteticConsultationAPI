namespace DieteticConsultationAPI.Entities;

public class Dietician
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Specialization { get; set; }
    public string ContactEmail { get; set; }
    public string ContactNumber { get; set; }
    public virtual List<Patient> Patients { get; set; } = new();
}