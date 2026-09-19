namespace Application.Models;

public enum CoffeeSize { Small = 1, Medium = 2, Large = 3 }
public enum CoffeeType { Hot = 1, Cold = 2 }
public sealed class CoffeeOption
{
    public Guid Id { get; private set; }
    public Guid CoffeeId { get; private set; }
    public CoffeeType Type { get; private set; }
    public CoffeeSize Size { get; private set; }
    public long PriceInCents { get; private set; }
    private CoffeeOption() { } // required by ef core
    public CoffeeOption(Guid coffeeId, CoffeeType type, CoffeeSize size, long priceInCents)
    {
        if (priceInCents < 0)
            throw new ArgumentOutOfRangeException(nameof(priceInCents), "Price cannot be negative.");
        Id = Guid.NewGuid();
        CoffeeId = coffeeId;
        Type = type;
        Size = size;
        PriceInCents = priceInCents;
    }

    public void Update(CoffeeType type, CoffeeSize size, long priceInCents)
    {
        if (priceInCents < 0)
            throw new ArgumentOutOfRangeException(nameof(priceInCents), "Price cannot be negative.");
        Type = type;
        Size = size;
        PriceInCents = priceInCents;
    }

    public void UpdatePriceInCents(long priceInCents)
    {
        if (priceInCents < 0)
            throw new ArgumentException("price in cents cannot be negative");
        PriceInCents = priceInCents;
    }
}
