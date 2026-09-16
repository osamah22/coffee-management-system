using Application.Database;
using Application.Models;
using Application.Options;
using Application.Repositories;
using Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SmtpOptions>(configuration.GetSection(SmtpOptions.Smtp));
        _addDb(services, configuration);
        services.AddValidatorsFromAssembly(
            typeof(IServiceCollectionExtensions).Assembly,
            ServiceLifetime.Scoped,
            includeInternalTypes: true);
        services.AddTransient<IEmailSender, EmailSender>();
        services.AddScoped<ICoffeeRepository, CoffeeRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICoffeeService, CoffeeService>();
        return services;
    }
    private static IServiceCollection _addDb(IServiceCollection services, IConfiguration configuration)
    {
        var conntectionString = configuration.GetConnectionString("DefaultConnection");
        if (conntectionString == null || conntectionString.IsWhiteSpace())
            throw new ArgumentException($"connection strings '{conntectionString}' is invalid");

        services.AddDbContext<ApplicationDbContext>(options =>
        options
        .UseNpgsql(conntectionString)
        .UseSnakeCaseNamingConvention());
        return services;
    }

}
