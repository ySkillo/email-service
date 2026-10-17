using EmailService.Domain.Email.Entities;

namespace EmailService.Application.Email.Interfaces;

public interface IEmailSender
{
    Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default);
}
