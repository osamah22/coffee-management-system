namespace Contracts.Responses;

public sealed record CoffeeResponse(Guid Id,
    string Name,
    string Description,
    long PriceInCents,
    string Type,
    string Slug);