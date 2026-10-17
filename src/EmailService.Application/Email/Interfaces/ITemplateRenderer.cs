using EmailService.Domain.Email.Entities;
using EmailService.Domain.Email.Enums;

namespace EmailService.Application.Email.Interfaces;

public interface ITemplateRenderer
{
    Task<string> RenderAsync(EmailTemplateType type, IReadOnlyDictionary<string, string> data, CancellationToken cancellationToken = default);
}
