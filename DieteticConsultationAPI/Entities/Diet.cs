using DieteticConsultationAPI.Models;

namespace DieteticConsultationAPI.Entities;

public class Diet
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int CalorificValue { get; set; }
    public string? ProhibitedProducts { get; set; }
    public string? RecommendedProducts   { get; set; }
    public int? PatientId { get; set; }
    public virtual Patient? Patient { get; set; }
    public virtual List<FileModel>? Files { get; set; }
    
    public static Diet For(DietDto dietDto) =>
        dietDto is null
        ? null
        : new()
        {
            Id = dietDto.Id,
            Name = dietDto.Name,
            Description = dietDto.Description,
            CalorificValue = dietDto.CalorificValue,
            ProhibitedProducts = dietDto.ProhibitedProducts,
            RecommendedProducts = dietDto.RecommendedProducts,
            Files = dietDto.Files?.Select(FileModel.For).ToList()
        };
}