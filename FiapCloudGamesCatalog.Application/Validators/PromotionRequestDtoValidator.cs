using FiapCloudGamesCatalog.Application.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiapCloudGamesCatalog.Application.Validators
{
    public class PromotionRequestDtoValidator : AbstractValidator<PromotionRequestDto>
    {
        public PromotionRequestDtoValidator()
        {
            RuleFor(x => x.DiscountPercentage)
                .InclusiveBetween(1, 100).WithMessage("Discount percentage must be between 1% and 100%.");

            RuleFor(x => x.StartDate)
                .LessThan(x => x.EndDate).WithMessage("Start date must be before end date.");

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date.");

            RuleFor(x => x.GameIds)
                .NotEmpty().WithMessage("At least one game must be included in the promotion.");
        }
    }
}
