using Application.Database;
using Application.Dtos;
using Application.Exceptions;
using Application.Exceptions.Base;
using Application.Models;
using Application.Repositories;
using FluentValidation;
using JasperFx.Core.Reflection;

namespace Application.Services;

public interface ICoffeeOptionService
{
    Task<Guid> CreateAsync(CreateCoffeeOptionDto command, CancellationToken ct);
    Task UpdateAsync(UpdateCoffeeOptionDto command, CancellationToken ct);
    Task DeleteAsync(Guid coffeeId, Guid id, CancellationToken ct);
    Task<CoffeeOption> GetAsync(Guid coffeeId, Guid id, CancellationToken ct);
    Task<IList<CoffeeOption>> GetAllAsync(Guid coffeeId, CancellationToken ct);
}

internal sealed class CoffeeOptionService(
    IUnitOfWork uow,
    ICoffeeRepository coffeeRepository,
    ICoffeeOptionRepository optionRepository,
    IValidator<CreateCoffeeOptionDto> createValidator,
    IValidator<UpdateCoffeeOptionDto> updateValidator) : ICoffeeOptionService
{
    public async Task<Guid> CreateAsync(CreateCoffeeOptionDto command, CancellationToken ct)
    {
        await createValidator.ValidateAndThrowAsync(command, ct);
        if (!await coffeeRepository.ExistsByIdAsync(command.CoffeeId, ct)) throw new CoffeeIdNotFoundException();
        var type = Enum.Parse<CoffeeType>(command.Type, true);
        var size = Enum.Parse<CoffeeSize>(command.Size, true);
        if (await optionRepository.ExistsAsync(command.CoffeeId, type, size, null, ct)) throw new CoffeeOptionExistsException();
        var option = new CoffeeOption(command.CoffeeId, type, size, command.PriceInCents);
        await optionRepository.CreateAsync(option, ct);
        if (await uow.SaveChangesAsync(ct) == 0) throw new InternalErrorException("coffee option could not be created");
        return option.Id;
    }

    public async Task UpdateAsync(UpdateCoffeeOptionDto command, CancellationToken ct)
    {
        await updateValidator.ValidateAndThrowAsync(command, ct);
        var option = await optionRepository.GetAsync(command.CoffeeId, command.Id, ct) ?? throw new CoffeeOptionNotFoundException();
        var type = Enum.Parse<CoffeeType>(command.Type, true);
        var size = Enum.Parse<CoffeeSize>(command.Size, true);
        if (await optionRepository.ExistsAsync(command.CoffeeId, type, size, command.Id, ct)) throw new CoffeeOptionExistsException();
        option.Update(type, size, command.PriceInCents);
        if (await uow.SaveChangesAsync(ct) == 0) throw new InternalErrorException("coffee option could not be updated");
    }

    public async Task DeleteAsync(Guid coffeeId, Guid id, CancellationToken ct)
    {
        var option = await optionRepository.GetAsync(coffeeId, id, ct) ?? throw new CoffeeOptionNotFoundException();
        await optionRepository.RemoveAsync(option, ct);
        if (await uow.SaveChangesAsync(ct) == 0) throw new InternalErrorException("coffee option could not be deleted");
    }

    public async Task<CoffeeOption> GetAsync(Guid coffeeId, Guid id, CancellationToken ct)
    {
        return await optionRepository.GetAsync(coffeeId, id, ct) ?? throw new CoffeeOptionNotFoundException();
    }

    public async Task<IList<CoffeeOption>> GetAllAsync(Guid coffeeId, CancellationToken ct)
    {
        if (!await coffeeRepository.ExistsByIdAsync(coffeeId, ct)) throw new CoffeeIdNotFoundException();
        var options = await optionRepository.GetAllAsync(coffeeId, ct);
        return options
            .ToList(); ;
    }
}
