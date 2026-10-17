using System.Net;
using System.Net.Mail;
using EmailService.Application.Email.Interfaces;
using EmailService.Domain.Email.Entities;
using EmailService.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EmailService.Infrastructure.Email;
public sealed class SmtpEmailSender : IEmailSender
{
    private readonly SmtpSettings _settings;
    private readonly ILogger<SmtpEmailSender> _logger;

    public SmtpEmailSender(IOptions<SmtpSettings> options, ILogger<SmtpEmailSender> logger)
    {
        _settings = options.Value;
        _logger = logger;
    }

    public async Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default)
    {
        using var mail = new MailMessage
        {
            From = new MailAddress(_settings.FromAddress, _settings.FromName),
            Subject = message.Subject,
            Body = message.Body,
            IsBodyHtml = message.IsHtml
        };
        mail.To.Add(message.To);

        // SmtpClient NÃO é thread-safe: criamos um por envio.
        using var client = new SmtpClient(_settings.Host, _settings.Port)
        {
            EnableSsl = _settings.EnableSsl,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_settings.User, _settings.Password),
            Timeout = 15_000
        };

        try
        {
            await client.SendMailAsync(mail, cancellationToken);
            _logger.LogInformation("E-mail enviado para {To} com assunto '{Subject}'", message.To, message.Subject);
        }
        catch (SmtpException ex)
        {
            _logger.LogError(ex, "Falha SMTP ao enviar e-mail para {To}", message.To);
            throw;
        }
    }
}
