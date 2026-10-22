# email-service

Microserviço de envio de e-mails em C# (.NET 8) seguindo Clean Architecture.

## Estrutura

```
src/
├── EmailService.Domain          # Entidades e enums (EmailTemplateType)
├── EmailService.Application     # Serviço de envio, assuntos por template
├── EmailService.Infrastructure  # SMTP (SmtpClient) e renderização dos templates HTML
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
dotnet user-secrets set "Smtp:User" "seu-usuario"
dotnet user-secrets set "Smtp:Password" "sua-senha"
dotnet user-secrets set "Smtp:FromAddress" "remetente@exemplo.com"
```

Seção `Smtp` do `appsettings.json`:

| Chave         | Descrição                   | Padrão            |
|---------------|-----------------------------|-------------------|
| `Host`        | Servidor SMTP               | `smtp.gmail.com`  |
| `Port`        | Porta                       | `587`             |
| `EnableSsl`   | Usa SSL/TLS                 | `true`            |
| `User`        | Usuário SMTP                | —                 |
| `Password`    | Senha SMTP                  | —                 |
| `FromAddress` | E-mail do remetente         | —                 |
| `FromName`    | Nome exibido do remetente   | `Happy Chat MSG`  |

## Chamadas da API

Base URL (desenvolvimento): `http://localhost:5290` (ou `https://localhost:7187`).

| Método | Rota               | Descrição                                   | Resposta            |
|--------|--------------------|---------------------------------------------|---------------------|
| `POST` | `/api/email/send`  | Envia um e-mail a partir de um template     | `202 Accepted`      |
| `GET`  | `/health`          | Health check do serviço                     | `200 Healthy`       |
| `GET`  | `/swagger`         | Documentação Swagger (somente Development)  | UI                  |

### `POST /api/email/send`

Renderiza o template escolhido, substitui os placeholders com os valores de `data` e envia o e-mail. O assunto é definido automaticamente pelo tipo do template.

**Body (JSON)**

| Campo          | Tipo                     | Obrigatório | Descrição                                                        |
|----------------|--------------------------|-------------|------------------------------------------------------------------|
| `to`           | `string`                 | Sim         | E-mail do destinatário                                           |
| `templateType` | `string` (enum)          | Sim         | `AccountOpening`, `Message`, `PasswordReset` ou `VerificationCode` |
| `data`         | `object<string, string>` | Não         | Valores dos placeholders do template (chaves não diferenciam maiúsculas/minúsculas) |

> `templateType` é enviado como **texto** (ex.: `"PasswordReset"`).

### Templates disponíveis

| `templateType`     | Assunto do e-mail                | Campos em `data`       |
|--------------------|----------------------------------|------------------------|
| `AccountOpening`   | Bem-vindo! Sua conta foi criada  | `Nome`, `Email`        |
| `Message`          | Você recebeu uma mensagem        | `Remetente`, `Mensagem`|
| `PasswordReset`    | Redefinição de senha             | `Nome`, `Codigo`       |
| `VerificationCode` | Seu código de verificação        | `Nome`, `Codigo`       |

Placeholders sem valor em `data` são substituídos por texto vazio. Os valores são HTML-encoded automaticamente.

### Exemplos

**Abertura de conta**

```bash
curl -X POST http://localhost:5290/api/email/send \
  -H "Content-Type: application/json" \
  -d '{
    "to": "usuario@exemplo.com",
    "templateType": "AccountOpening",
    "data": { "Nome": "Maria", "Email": "usuario@exemplo.com" }
  }'
```

**Nova mensagem**

```bash
curl -X POST http://localhost:5290/api/email/send \
  -H "Content-Type: application/json" \
  -d '{
    "to": "usuario@exemplo.com",
    "templateType": "Message",
    "data": { "Remetente": "João", "Mensagem": "Oi, tudo bem?" }
  }'
```

**Redefinição de senha**

```bash
curl -X POST http://localhost:5290/api/email/send \
  -H "Content-Type: application/json" \
  -d '{
    "to": "usuario@exemplo.com",
    "templateType": "PasswordReset",
    "data": { "Nome": "Maria", "Codigo": "123456" }
  }'
```

**Código de verificação**

```bash
curl -X POST http://localhost:5290/api/email/send \
  -H "Content-Type: application/json" \
  -d '{
    "to": "usuario@exemplo.com",
    "templateType": "VerificationCode",
    "data": { "Nome": "Maria", "Codigo": "482913" }
  }'
```

### Respostas

| Status | Quando                                                              |
|--------|---------------------------------------------------------------------|
| `202`  | E-mail aceito e enviado ao servidor SMTP                            |
| `400`  | Body inválido (JSON malformado ou `templateType` desconhecido)      |
| `500`  | Falha interna (ex.: `to` vazio, template não encontrado, erro SMTP) |

### Health check

```bash
curl http://localhost:5290/health
```

## Licença

[MIT](LICENSE)
