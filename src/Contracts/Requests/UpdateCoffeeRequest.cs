namespace Contracts.Requests;

public sealed record UpdateCoffeeRequest(
    string Name,
    string Description,
    long PriceInCents,
    string Type
);