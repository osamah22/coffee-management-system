using Application.Models;
using Contracts.Responses;

namespace Api.Mappers;

public static class CoffeeMapper
{
    public static CoffeeResponse ToResponse(this Coffee coffee)
    {
        return new CoffeeResponse(coffee.Id,
            coffee.Name,
            coffee.Description,
            coffee.Options.Select(x => new CoffeeOptionResponse(x.Id, x.Type.ToString(), x.Size.ToString(), x.PriceInCents)),
            coffee.Slug);
    }
}
