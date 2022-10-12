using AutoMapper;
using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Models;
using FileModel = DieteticConsultationAPI.Entities.FileModel;

namespace DieteticConsultationAPI
{
    public class DieteticConsultationMappingProfile : Profile
    {
        public DieteticConsultationMappingProfile()
        {
            CreateMap<Dietician, DieticianDto>();
            CreateMap<Patient, PatientDto>();
            CreateMap<Diet, DietDto>();
            CreateMap<FileModel, FileModelDto>();
        }
    }
}
