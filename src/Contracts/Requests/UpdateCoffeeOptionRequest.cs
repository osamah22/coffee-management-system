namespace Contracts.Requests;

public sealed record UpdateCoffeeOptionRequest(string Type, string Size, long PriceInCents);
