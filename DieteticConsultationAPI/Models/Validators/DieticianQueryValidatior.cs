using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Models.Pagination;
using FluentValidation;

namespace DieteticConsultationAPI.Models.Validators
{
    public class DieticianQueryValidatior : AbstractValidator<DieticianQuery>
    {
        private int[] allowedPageSizes = new[] { 5, 10, 15 };

        private string[] allowedSortByColumnNames =
        {
            nameof(Dietician.FirstName),
            nameof(Dietician.LastName),
            nameof(Dietician.Specialization)
        };

        public DieticianQueryValidatior()
        {
            RuleFor(d => d.PageNumber).GreaterThanOrEqualTo(1);

            RuleFor(d => d.PageSize).Custom((value, context) =>
            {
                if (!allowedPageSizes.Contains(value))
                {
                    context.AddFailure("PageSize", $"PageSize must in [{string.Join(",", allowedPageSizes)}]");
                }
            });

            RuleFor(d => d.SortBy)
                .Must(value => string.IsNullOrEmpty(value) || allowedSortByColumnNames.Contains(value))
                .WithMessage($"Sort by is optional, or must be in [{string.Join(",", allowedSortByColumnNames)}]");
        }
    }
}
