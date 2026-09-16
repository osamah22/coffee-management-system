namespace Application.Exceptions.Base;

public abstract class NotFoundException(string Message) : Exception(Message) { }