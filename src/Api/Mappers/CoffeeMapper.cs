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
            coffee.PriceInCents,
            coffee.Type.ToString(),
            coffee.Slug);
    }
}