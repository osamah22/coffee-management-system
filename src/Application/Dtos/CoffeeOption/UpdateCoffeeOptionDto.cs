namespace Application.Dtos;

public sealed record UpdateCoffeeOptionDto(Guid CoffeeId, Guid Id, string Type, string Size, long PriceInCents);
