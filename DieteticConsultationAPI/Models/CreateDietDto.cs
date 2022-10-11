﻿using DieteticConsultationAPI.Entities;

namespace DieteticConsultationAPI.Models
{
    public class CreateDietDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int CalorificValue { get; set; }
        public string? ProhibitedProducts { get; set; }
        public string? RecommendedProducts { get; set; }
        public int? PatientId { get; set; }
        public virtual List<FileModelDto>? Files { get; set; } 
    }
}