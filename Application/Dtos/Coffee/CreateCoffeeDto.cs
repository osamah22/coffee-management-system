namespace Application.Dtos;

public record CreateCoffeeDto(string Name,
    string Description,
    long PriceInCents,
    string Type);