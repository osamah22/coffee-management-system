using Application.Models;
using FluentValidation;

namespace Application.Dtos;

internal sealed class CreateCoffeeOptionDtoValidator : AbstractValidator<CreateCoffeeOptionDto>
{
    public CreateCoffeeOptionDtoValidator()
    {
        RuleFor(x => x.Type).Must(x => Enum.TryParse<CoffeeType>(x, true, out _)).WithMessage("Invalid coffee type.");
        RuleFor(x => x.Size).Must(x => Enum.TryParse<CoffeeSize>(x, true, out _)).WithMessage("Invalid coffee size.");
        RuleFor(x => x.PriceInCents).GreaterThanOrEqualTo(0);
    }
}

internal sealed class UpdateCoffeeOptionDtoValidator : AbstractValidator<UpdateCoffeeOptionDto>
{
    public UpdateCoffeeOptionDtoValidator()
    {
        RuleFor(x => x.Type).Must(x => Enum.TryParse<CoffeeType>(x, true, out _)).WithMessage("Invalid coffee type.");
        RuleFor(x => x.Size).Must(x => Enum.TryParse<CoffeeSize>(x, true, out _)).WithMessage("Invalid coffee size.");
        RuleFor(x => x.PriceInCents).GreaterThanOrEqualTo(0);
    }
}
