# email-service

Microserviço de envio de e-mails em C# (.NET 8) seguindo Clean Architecture.

## Estrutura

```
src/
├── EmailService.Domain          # Entidades e regras de negócio
├── EmailService.Application     # Casos de uso (MediatR, FluentValidation)
├── EmailService.Infrastructure  # Implementações (SMTP, provedores de e-mail)
└── EmailService.API             # ASP.NET Core Web API
```

Dependências: `API → Application, Infrastructure` · `Infrastructure → Application, Domain` · `Application → Domain`.

## Como rodar

Pré-requisito: [.NET SDK 9+](https://dotnet.microsoft.com/download) (necessário para o `.slnx`; os projetos têm como alvo `net8.0`).

```bash
git clone git@github.com:ySkillo/email-service.git
cd email-service
dotnet run --project src/EmailService.API
```

- Swagger: http://localhost:5290/swagger
- Health check: http://localhost:5290/health

## Configuração e segredos

Não versione credenciais. Use [user-secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets) ou variáveis de ambiente:

```bash
cd src/EmailService.API
dotnet user-secrets init
dotnet user-secrets set "Smtp:Password" "sua-senha"
```

## Licença

[MIT](LICENSE)
