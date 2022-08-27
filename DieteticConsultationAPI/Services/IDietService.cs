using DieteticConsultationAPI.Models;
namespace DieteticConsultationAPI.Services;

public interface IDietService
{
    int Create(CreateDietDto dto);
    IEnumerable<DietDto> GetAll();

}