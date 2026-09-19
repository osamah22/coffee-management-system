namespace Contracts.Responses;

public sealed record CoffeeResponse(Guid Id,
    string Name,
    string Description,
    IEnumerable<CoffeeOptionResponse> Options,
    string Slug);

public sealed record CoffeeOptionResponse(Guid Id, string Type, string Size, long PriceInCents);
