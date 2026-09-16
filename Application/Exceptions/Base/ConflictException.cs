namespace Application.Exceptions.Base;

public abstract class ConflictException(string Message) : Exception(Message) { }