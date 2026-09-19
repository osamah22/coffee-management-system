using System.Text.RegularExpressions;

namespace Application.Models;

public sealed class Coffee
{
    private readonly List<CoffeeOption> _options = [];
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string Slug { get; private set; } = null!;
    public IReadOnlyCollection<CoffeeOption> Options => _options;
    private Coffee() { } // requred by EF core
    public Coffee(string name, string description)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Slug = GenerateSlug();
    }

    public void UpdateCoffee(string name, string description)
    {
        Name = name;
        Description = description;
        Slug = GenerateSlug();
    }
    public CoffeeOption AddOption(CoffeeType type, CoffeeSize size, long priceInCents)
    {
        if (_options.Any(x => x.Type == type && x.Size == size))
            throw new InvalidOperationException("A coffee option with this type and size already exists.");
        var option = new CoffeeOption(Id, type, size, priceInCents);
        _options.Add(option);
        return option;
    }

    private string GenerateSlug()
    {
        var result = Name.ToLowerInvariant().Trim();

        result = Regex.Replace(result, @"[^a-z0-9\s-]", "");
        result = Regex.Replace(result, @"\s+", "-");
        result = Regex.Replace(result, @"-+", "-");

        return result.Trim('-');
    }

}
