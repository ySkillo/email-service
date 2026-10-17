using EmailService.Domain.Email.Enums;

namespace EmailService.Application.Email.DTOs;

public record SendEmailRequest(
    string To,
    EmailTemplateType  TemplateType,
    Dictionary<string, string>? Data);
