using Application.Exceptions;
using Application.Exceptions.Base;
using Contracts.Responses;
using FluentValidation;
using Microsoft.Extensions.Validation;

namespace Api.Middlewares;

public sealed class ExceptionHandlerMiddleware
{

    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            var errors = ex.Errors.Select(x =>
             new ValidaitonError
             {
                 PropertyName = x.PropertyName,
                 Code = x.ErrorCode,
                 Message = x.ErrorMessage

             });
            var response = new ValidationFailureResponse { Errors = errors };
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (NotFoundException ex)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new { error = ex.Message });
        }
        catch (ConflictException ex)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new { error = ex.Message });
        }
        catch (InternalErrorException ex)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new { error = ex.Message });
        }
    }
}