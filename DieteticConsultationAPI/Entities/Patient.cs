﻿using DieteticConsultationAPI.Models;
namespace DieteticConsultationAPI.Entities;

public class Patient
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
    public int? CreatedById { get; set; }
    public virtual User CreatedBy { get; set; }
    public int DieticianId { get; set; }
    public virtual Dietician Dietician { get; set; }
    public virtual Diet? Diet { get; set; }
    
    public static Patient For(PatientDto patientdto) =>
        new()
        {
            Id = patientdto.Id,
            FirstName = patientdto.FirstName,
            LastName = patientdto.LastName,
            ContactEmail = patientdto.ContactEmail,
            ContactNumber = patientdto.ContactNumber,
            Sex = patientdto.Sex,
            Weight = patientdto.Weight,
            Height = patientdto.Height,
            Age = patientdto.Age,
            Diet = Diet.For(patientdto.Diet)
        };
}