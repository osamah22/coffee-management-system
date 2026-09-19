namespace Contracts.Requests;

public sealed record CreateCoffeeOptionRequest(string Type, string Size, long PriceInCents);
