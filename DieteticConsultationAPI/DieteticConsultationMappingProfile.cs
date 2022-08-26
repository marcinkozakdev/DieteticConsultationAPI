﻿using AutoMapper;
using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Models;

namespace DieteticConsultationAPI
{
    public class DieteticConsultationMappingProfile : Profile
    {
        public DieteticConsultationMappingProfile()
        {
            CreateMap<Dietician, DieticianDto>();
            CreateMap<Patient, PatientDto>();
            CreateMap<Diet, DietDto>();

            CreateMap<AddDieticianDto, Dietician>();
            CreateMap<AddPatientDto, Patient>();
            CreateMap<CreateDietDto, Diet>();

            CreateMap<UpdateDieticianDto, Dietician>();
            CreateMap<UpdatePatientDto, Patient>();
            CreateMap<UpdateDietDto, Diet>();
        }
    }
}