namespace Application.Exceptions;

using Application.Exceptions.Base;

public sealed class CoffeeIdNotFoundException() : NotFoundException("COFFEE_ID_NOT_FOUND") { }

public sealed class CoffeeSlugNotFoundException() : NotFoundException("COFFEE_SLUG_NOT_FOUND") { }

public sealed class CoffeeSlugExistsException() : ConflictException("SLUG_ALREADY_EXISTS") { }
public sealed class CoffeeOptionNotFoundException() : NotFoundException("COFFEE_OPTION_NOT_FOUND") { }
public sealed class CoffeeOptionExistsException() : ConflictException("COFFEE_OPTION_ALREADY_EXISTS") { }
