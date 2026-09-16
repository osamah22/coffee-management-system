namespace Contracts.Responses;

public class ValidationFailureResponse
{
    public required IEnumerable<ValidaitonError> Errors { get; init; }
}

public class ValidaitonError
{
    public required string PropertyName { get; init; }
    public required string Code { get; set; }
    public required string Message { get; set; }
}