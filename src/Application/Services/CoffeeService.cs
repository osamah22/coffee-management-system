using Application.Database;
using Application.Dtos;
using Application.Exceptions;
using Application.Exceptions.Base;
using Application.Models;
using Application.Repositories;
using FluentValidation;

namespace Application.Services;

internal sealed class CoffeeService : ICoffeeService
{
    private readonly IUnitOfWork _uow;
    private readonly ICoffeeRepository _coffeeRepository;
    private readonly IValidator<CreateCoffeeDto> _createValidator;
    private readonly IValidator<UpdateCoffeeDto> _updateValidator;
    public CoffeeService(IUnitOfWork uow,
        ICoffeeRepository coffeeRepository,
        IValidator<CreateCoffeeDto> createValidator,
        IValidator<UpdateCoffeeDto> updateValidator)
    {
        _uow = uow;
        _coffeeRepository = coffeeRepository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<Guid?> CreateAsync(CreateCoffeeDto command, CancellationToken ct = default)
    {
        await _createValidator.ValidateAndThrowAsync(command, ct);
        var coffee = new Coffee(command.Name, command.Description);

        if (await _coffeeRepository.ExistsBySlugAsync(coffee.Slug, ct))
            throw new CoffeeSlugExistsException();
        await _coffeeRepository.CreateAsync(coffee, ct);
        if (await _uow.SaveChangesAsync(ct) == 0)
            throw new InternalErrorException("coffee could not be created");
        return coffee.Id;
    }

    public async Task UpdateAsync(UpdateCoffeeDto command, CancellationToken ct = default)
    {
        await _updateValidator.ValidateAndThrowAsync(command, ct);
        var coffee = await _coffeeRepository.GetByIdAsync(command.Id, ct);
        if (coffee == null)
            throw new CoffeeIdNotFoundException();
        coffee.UpdateCoffee(command.Name, command.Description);
        await _coffeeRepository.UpdateAsync(coffee, ct);
        if (await _uow.SaveChangesAsync(ct) == 0)
            throw new InternalErrorException("coffee could not be updated");
    }

    public async Task DeleteCoffeeAsync(Guid id, CancellationToken ct)
    {
        var coffee = await _coffeeRepository.GetByIdAsync(id, ct);
        if (coffee is null)
            throw new CoffeeIdNotFoundException();
        await _coffeeRepository.RemoveAsync(coffee, ct);

        if (await _uow.SaveChangesAsync(ct) == 0)
            throw new InternalErrorException("coffee could not be deleted");
    }

    public async Task<Coffee?> GetByIdOrSlugAsync(string idOrSlug,
        CancellationToken ct)
    {
        Coffee? coffee;
        if (Guid.TryParse(idOrSlug, out var id))
        {
            coffee = await _coffeeRepository.GetByIdAsync(id, ct);
            if (coffee is null)
                throw new CoffeeIdNotFoundException();
        }
        coffee = await _coffeeRepository.GetBySlugAsync(idOrSlug, ct);
        if (coffee is null)
            throw new CoffeeSlugNotFoundException();

        return coffee;
    }

    public async Task<IList<Coffee>> GetAllAsync(CancellationToken ct = default)
    {
        return await _coffeeRepository.GetAllAsync(c => true, ct);
    }
}
