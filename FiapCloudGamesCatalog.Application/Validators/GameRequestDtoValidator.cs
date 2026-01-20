using FiapCloudGamesCatalog.Application.Dtos;
using FiapCloudGamesCatalog.Domain.Enums;
using FluentValidation;

namespace FiapCloudGamesCatalog.Application.Validators;

public class GameRequestDtoValidator : AbstractValidator<GameRequestDto>
{
    public GameRequestDtoValidator()
    {
        RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Game title is required.")
                .MaximumLength(100).WithMessage("Title must have a maximum of 100 characters.");

        RuleFor(x => x.Genre)
            .NotEmpty().WithMessage("Genre is required.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(x => x.Developer)
            .NotEmpty().WithMessage("Developer name is required.");

        RuleFor(x => x.Distributor)
            .NotEmpty().WithMessage("Distributor name is required.");

        RuleFor(x => x.GamePlatforms)
            .NotEmpty().WithMessage("At least one platform must be specified.")
            .Must(AllPlatformsAreValid)
            .WithMessage("One or more specified platforms are invalid.");

        RuleFor(x => x.GameVersion)
            .NotEmpty().WithMessage("Game version is required.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
    }

    private bool AllPlatformsAreValid(IList<string> platforms)
    {
        try
        {
            var platformsEnum = platforms
                .Select(s => (GamePlatformEnum)Enum.Parse(typeof(GamePlatformEnum), s))
                .ToList();

            var validValues = Enum.GetValues(typeof(GamePlatformEnum)).Cast<GamePlatformEnum>();
            return platformsEnum.All(p => validValues.Contains(p));
        }
        catch (Exception)
        {
            return false;
        }
    }
}
