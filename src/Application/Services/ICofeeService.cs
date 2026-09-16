using Application.Dtos;
using Application.Models;

namespace Application.Services;

public interface ICoffeeService
{
    public Task<Guid?> CreateAsync(CreateCoffeeDto command, CancellationToken ct);
    public Task UpdateAsync(UpdateCoffeeDto command, CancellationToken ct);
    public Task DeleteCoffeeAsync(Guid id, CancellationToken ct);
    public Task<Coffee?> GetByIdOrSlugAsync(string slugOrId, CancellationToken ct);
    public Task<IList<Coffee>> GetAllAsync(CancellationToken ct);
}