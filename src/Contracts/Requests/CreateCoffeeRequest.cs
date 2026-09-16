namespace Contracts.Requests;

public record CreateCoffeeRequest(string Name,
    string Description,
    long PriceInCents,
    string Type);