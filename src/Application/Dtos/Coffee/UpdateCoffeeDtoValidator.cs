using FluentValidation;

namespace Application.Dtos;

internal sealed class UpdateCoffeeDtoValidator : AbstractValidator<UpdateCoffeeDto>
{
    public UpdateCoffeeDtoValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(120)
            .WithErrorCode("COFFEE_NAME_TO_LARGE");

        RuleFor(x => x.Description)
            .MaximumLength(400)
            .WithErrorCode("COFFEE_DESCRIPTION_TO_LARGE");

    }
}
