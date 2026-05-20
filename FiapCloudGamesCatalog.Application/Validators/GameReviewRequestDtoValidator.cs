using FiapCloudGamesCatalog.Application.Dtos;
using FluentValidation;

namespace FiapCloudGamesCatalog.Application.Validators;

public class GameReviewRequestDtoValidator : AbstractValidator<GameReviewRequestDto>
{
    public GameReviewRequestDtoValidator()
    {
        RuleFor(x => x.Rating).InclusiveBetween(1, 5);
        RuleFor(x => x.Comment).MaximumLength(2000);
    }
}
