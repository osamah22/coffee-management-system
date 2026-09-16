using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Application.Models;

public enum CoffeeType
{
    Cold,
    Hot
}
public class Coffee
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public long PriceInCents { get; private set; }
    public string Slug { get; private set; }
    public CoffeeType Type { get; set; }
    private Coffee() { } // requred by EF core
    public Coffee(string name, string description, long priceInCents, CoffeeType type)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        PriceInCents = priceInCents;
        Type = type;
        Slug = GenerateSlug();
    }

    public void Update(string name, string description, long priceInCents, CoffeeType type)
    {
        Name = name;
        Description = description;
        PriceInCents = priceInCents;
        Type = type;
        Slug = GenerateSlug();
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