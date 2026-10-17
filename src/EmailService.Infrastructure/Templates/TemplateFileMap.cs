using EmailService.Domain.Email.Enums;

namespace EmailService.Infrastructure.Templates;

public class TemplateFileMap
{
    public static string FileFor(EmailTemplateType type) => type switch
    {
        EmailTemplateType.AccountOpening => "AccountOpening.html",
        EmailTemplateType.Message => "Message.html",
        EmailTemplateType.PasswordReset => "PasswordReset.html",
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Template html não suportado.")
    };
}
