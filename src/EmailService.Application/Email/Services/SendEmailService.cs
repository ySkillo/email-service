using EmailService.Application.Email.DTOs;
using EmailService.Application.Email.Interfaces;
using EmailService.Domain.Email.Entities;

namespace EmailService.Application.Email.Services;

public class SendEmailService : ISendEmailService
{
    private readonly ITemplateRenderer _renderer;
    private readonly IEmailSender _sender;

    public SendEmailService(ITemplateRenderer renderer, IEmailSender sender)
    {
        _renderer = renderer;
        _sender = sender;
    }

    public async Task SendAsync(SendEmailRequest request, CancellationToken cancellationToken = default)
    {
        if(string.IsNullOrEmpty(request.To))
            throw new ArgumentNullException("O destinatário é obrigatótio", nameof(request.To));

        var body = await _renderer.RenderAsync(
            request.TemplateType,
            request.Data ?? new Dictionary<string, string>(),
            cancellationToken
        );

        var message = new EmailMessage(
            To: request.To,
            Subject: TemplateSubjects.For(request.TemplateType),
            Body: body
        );

        await _sender.SendAsync(message, cancellationToken);
    }
}
