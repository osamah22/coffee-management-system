namespace Application.Exceptions.Base;

public sealed class InternalErrorException(string Message) : Exception(Message) { }