using EmailService.Domain.Email.Enums;

namespace EmailService.Application.Email.Services;

public class TemplateSubjects
{
    public static string For(EmailTemplateType type) => type switch
    {
        EmailTemplateType.AccountOpening => "Bem-vindo! Sua conta foi criada",
        EmailTemplateType.Message => "Você recebeu uma mensagem",
        EmailTemplateType.PasswordReset => "Redefinição de senha",
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Template não suportado.")
    };
}
