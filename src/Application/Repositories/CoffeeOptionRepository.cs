using Application.Models;
using ImTools;
using Microsoft.EntityFrameworkCore;

namespace Application.Repositories;

public interface ICoffeeOptionRepository
{
    Task CreateAsync(CoffeeOption option, CancellationToken ct);
    Task<CoffeeOption?> GetAsync(Guid coffeeId, Guid id, CancellationToken ct);
    Task<IList<CoffeeOption>> GetAllAsync(Guid coffeeId, CancellationToken ct);
    Task<bool> ExistsAsync(Guid coffeeId, CoffeeType type, CoffeeSize size, Guid? exceptId, CancellationToken ct);
    Task RemoveAsync(CoffeeOption option, CancellationToken ct);
}

public sealed class CoffeeOptionRepository(ApplicationDbContext context) : ICoffeeOptionRepository
{
    public async Task CreateAsync(CoffeeOption option, CancellationToken ct) => await context.Set<CoffeeOption>().AddAsync(option, ct);
    public Task<CoffeeOption?> GetAsync(Guid coffeeId, Guid id, CancellationToken ct) =>
        context.Set<CoffeeOption>().FirstOrDefaultAsync(x => x.CoffeeId == coffeeId && x.Id == id, ct);
    public async Task<IList<CoffeeOption>> GetAllAsync(Guid coffeeId, CancellationToken ct)
    {
        return await context.Set<CoffeeOption>()
            .Where(x => x.CoffeeId == coffeeId)
            .OrderBy(x => x.Type)
            .ThenBy(x => x.Size)
            .ToListAsync(ct);
    }
    public Task<bool> ExistsAsync(Guid coffeeId, CoffeeType type, CoffeeSize size, Guid? exceptId, CancellationToken ct) =>
        context.Set<CoffeeOption>().AnyAsync(x => x.CoffeeId == coffeeId && x.Type == type && x.Size == size && (!exceptId.HasValue || x.Id != exceptId.Value), ct);
    public Task RemoveAsync(CoffeeOption option, CancellationToken ct) { context.Set<CoffeeOption>().Remove(option); return Task.CompletedTask; }
}
