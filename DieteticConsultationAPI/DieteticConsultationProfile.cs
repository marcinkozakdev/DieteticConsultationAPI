using AutoMapper;
using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Models;

namespace DieteticConsultationAPI
{
    public class DieteticConsultationProfile : Profile
    {
        public DieteticConsultationProfile()
        {
            CreateMap<Dietician, DieticianDto>();

            CreateMap<Diet, DietDto>();

            CreateMap<Patient, PatientDto>();

            CreateMap<AddDieticianDto, Dietician>();

            CreateMap<CreateDietDto, Diet>();

            CreateMap<AddPatientDto, Patient>();

        }
    }
}
