using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Exceptions;
using DieteticConsultationAPI.Extensions;

namespace DieteticConsultationAPI.Models
{
    public class DietDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CalorificValue { get; set; }
        public string ProhibitedProducts { get; set; }
        public string RecommendedProducts { get; set; }
        public virtual List<FileModelDto> Files { get; set; }

        public static DietDto For(Diet diet)
            =>
            diet is null 
            ? null
            : new()
            {
                Id = diet.Id,
                Name = diet.Name,
                Description = diet.Description,
                CalorificValue = diet.CalorificValue,
                ProhibitedProducts = diet.ProhibitedProducts,
                RecommendedProducts = diet.RecommendedProducts,
                Files = diet.Files?.Select(FileModelDto.For).ToList()
            };

        public static async ValueTask<DietDto> BindAsync(HttpContext context)
        {
            var form = await context.Request.ReadFormAsync();

            form.TryGetValue(nameof(Id), out var id);
            form.TryGetValue(nameof(Name), out var name);
            form.TryGetValue(nameof(Description), out var description);
            form.TryGetValue(nameof(CalorificValue), out var calorificValue);
            form.TryGetValue(nameof(ProhibitedProducts), out var prohibitedProducts);
            form.TryGetValue(nameof(RecommendedProducts), out var recommendedProducts);

            var result = new DietDto
            {
                Id = int.TryParse(id, out var parsedId) ? parsedId : 0,
                Name = name,
                Description = description,
                CalorificValue = int.TryParse(calorificValue, out var calorificValueParse) ? calorificValueParse : 0,
                ProhibitedProducts = prohibitedProducts,
                RecommendedProducts = recommendedProducts,
                Files = form.Deserialize<List<FileModelDto>>(nameof(Files))
            };

            return await ValueTask.FromResult(result);
        }
    }
}