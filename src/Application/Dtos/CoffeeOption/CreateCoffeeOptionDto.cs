namespace Application.Dtos;

public sealed record CreateCoffeeOptionDto(Guid CoffeeId, string Type, string Size, long PriceInCents);
