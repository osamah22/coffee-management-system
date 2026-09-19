using FluentValidation;

namespace Application.Dtos;

internal sealed class CreateCoffeeDtoValidator : AbstractValidator<CreateCoffeeDto>
{
    public CreateCoffeeDtoValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(120)
            .WithErrorCode("COFFEE_TITLE_TO_LARGE");

        RuleFor(x => x.Description)
            .MaximumLength(400)
            .WithErrorCode("COFFEE_DESCRIPTION_TO_LARGE");

    }
}
