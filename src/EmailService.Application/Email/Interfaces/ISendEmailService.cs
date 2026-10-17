using EmailService.Application.Email.DTOs;
using EmailService.Domain.Email.Entities;

namespace EmailService.Application.Email.Interfaces;

public interface ISendEmailService
{
    Task SendAsync(SendEmailRequest request, CancellationToken cancellationToken = default);
}
