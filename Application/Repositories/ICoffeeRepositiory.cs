using System.Linq.Expressions;
using Application.Models;

namespace Application.Repositories;

public interface ICoffeeRepository
{
    public Task CreateAsync(Coffee coffee, CancellationToken ct);
    public Task UpdateAsync(Coffee coffee, CancellationToken ct);
    public Task RemoveAsync(Coffee coffee, CancellationToken ct);
    public Task<bool> ExistsByIdAsync(Guid id, CancellationToken ct);
    public Task<bool> ExistsBySlugAsync(string Slug, CancellationToken ct);
    public Task<Coffee?> GetByIdAsync(Guid id, CancellationToken ct);
    public Task<Coffee?> GetBySlugAsync(string slug, CancellationToken ct);
    public Task<IList<Coffee>> GetAllAsync(
        Expression<Func<Coffee, bool>> expression,
        CancellationToken cancellationToken);
}