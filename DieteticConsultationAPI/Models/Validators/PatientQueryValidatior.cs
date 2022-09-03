using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Models.Pagination;
using FluentValidation;

namespace DieteticConsultationAPI.Models.Validators
{
    public class PatientQueryValidatior : AbstractValidator<PatientQuery>
    {
        private int[] allowedPageSizes = new[] { 5, 10, 15, 20, 25 };

        private string[] allowedSortByColumnNames =
        {
            nameof(Patient.FirstName),
            nameof(Patient.LastName)
        };

        public PatientQueryValidatior()
        {
            RuleFor(p => p.PageNumber).GreaterThanOrEqualTo(1);

            RuleFor(p => p.PageSize).Custom((value, context) =>
            {
                if (!allowedPageSizes.Contains(value))
                {
                    context.AddFailure("PageSize", $"PageSize must in [{string.Join(",", allowedPageSizes)}]");
                }
            });

            RuleFor(p => p.SortBy)
                .Must(value => string.IsNullOrEmpty(value) || allowedSortByColumnNames.Contains(value))
                .WithMessage($"Sort by is optional, or must be in [{string.Join(",", allowedSortByColumnNames)}]");
        }
    }
}
