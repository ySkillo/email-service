namespace EmailService.Domain.Email.Entities;

public record EmailMessage(string To,
    string Subject,
    string Body,
    bool IsHtml = true);
