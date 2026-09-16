namespace Application.Dtos;

public record UpdateCoffeeDto(Guid Id,
    string Name,
    string Description,
    long PriceInCents,
    string Type);