using EmailService.Application.Email.Interfaces;
using EmailService.Infrastructure.Email;
using EmailService.Infrastructure.Settings;
using EmailService.Infrastructure.Templates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmailService.Infraestructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfraestructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SmtpSettings>(configuration.GetSection(SmtpSettings.SectionName));

        services.AddSingleton<ITemplateRenderer, HtmlTemplateRenderer>();
        services.AddScoped<IEmailSender, SmtpEmailSender>();

        return services;
    }
}
