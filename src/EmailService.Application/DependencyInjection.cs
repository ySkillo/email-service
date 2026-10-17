using EmailService.Application.Email.Interfaces;
using EmailService.Application.Email.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EmailService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ISendEmailService, SendEmailService>();
        return services;
    }
}
