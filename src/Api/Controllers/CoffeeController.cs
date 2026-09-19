using Api.Constants;
using Api.Mappers;
using Application.Dtos;
using Application.Services;
using Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Authorize(Roles = AuthConstants.Roles.Manager)]
public sealed class CoffeeController : ControllerBase
{
    private readonly ICoffeeService _coffeeService;
    public CoffeeController(ICoffeeService coffeeService)
    {
        _coffeeService = coffeeService;
    }

    [HttpPost(ApiEndpoints.Coffee.V1.Create)]
    public async Task<IActionResult> Create([FromBody] CreateCoffeeRequest request,
        CancellationToken ct)
    {
        var cmd = new CreateCoffeeDto(request.Name, request.Description);
        var id = await _coffeeService.CreateAsync(cmd, ct);
        return CreatedAtAction(nameof(Get), new { idOrSlug = id.ToString() }, null);
    }

    [HttpPut(ApiEndpoints.Coffee.V1.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id,
        [FromBody] UpdateCoffeeRequest request,
        CancellationToken ct)
    {
        var cmd = new UpdateCoffeeDto(id, request.Name, request.Description);
        await _coffeeService.UpdateAsync(cmd, ct);
        return NoContent();
    }

    [HttpDelete(ApiEndpoints.Coffee.V1.Delete)]
    public async Task<IActionResult> DeleteCoffeeRequest([FromRoute] Guid id, CancellationToken ct)
    {
        await _coffeeService.DeleteCoffeeAsync(id, ct);
        return NoContent();
    }

    [AllowAnonymous]
    [HttpGet(ApiEndpoints.Coffee.V1.Get)]
    public async Task<IActionResult> Get([FromRoute] string idOrSlug, CancellationToken ct)
    {
        var coffee = await _coffeeService.GetByIdOrSlugAsync(idOrSlug, ct);
        return (coffee is not null) ?
            Ok(coffee.ToResponse()) :
            NotFound("coffee was not found");
    }

    [AllowAnonymous]
    [HttpGet(ApiEndpoints.Coffee.V1.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var coffees = await _coffeeService.GetAllAsync(ct);
        var response = coffees.Select(c => c.ToResponse());

        return Ok(response);
    }
}
