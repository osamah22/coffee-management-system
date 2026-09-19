using Api.Constants;
using Application.Dtos;
using Application.Services;
using Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Authorize(Roles = AuthConstants.Roles.Manager)]
public sealed class CoffeeOptionController(ICoffeeOptionService service) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet(ApiEndpoints.Coffee.V1.Options)]
    public async Task<IActionResult> GetAll(Guid coffeeId, CancellationToken ct) =>
        Ok((await service.GetAllAsync(coffeeId, ct)).Select(ToResponse));

    [AllowAnonymous]
    [HttpGet(ApiEndpoints.Coffee.V1.Option)]
    public async Task<IActionResult> Get(Guid coffeeId, Guid optionId, CancellationToken ct) =>
        Ok(ToResponse(await service.GetAsync(coffeeId, optionId, ct)));

    [HttpPost(ApiEndpoints.Coffee.V1.Options)]
    public async Task<IActionResult> Create(Guid coffeeId, CreateCoffeeOptionRequest request, CancellationToken ct)
    {
        var id = await service.CreateAsync(new CreateCoffeeOptionDto(coffeeId, request.Type, request.Size, request.PriceInCents), ct);
        return CreatedAtAction(nameof(Create), new { coffeeId, optionId = id }, new { id });
    }

    [HttpPut(ApiEndpoints.Coffee.V1.Option)]
    public async Task<IActionResult> Update(Guid coffeeId, Guid optionId, UpdateCoffeeOptionRequest request, CancellationToken ct)
    {
        await service.UpdateAsync(new UpdateCoffeeOptionDto(coffeeId, optionId, request.Type, request.Size, request.PriceInCents), ct);
        return NoContent();
    }

    [HttpDelete(ApiEndpoints.Coffee.V1.Option)]
    public async Task<IActionResult> Delete(Guid coffeeId, Guid optionId, CancellationToken ct)
    {
        await service.DeleteAsync(coffeeId, optionId, ct);
        return NoContent();
    }

    private static object ToResponse(Application.Models.CoffeeOption option) =>
        new { option.Id, Type = option.Type.ToString(), Size = option.Size.ToString(), option.PriceInCents };
}
