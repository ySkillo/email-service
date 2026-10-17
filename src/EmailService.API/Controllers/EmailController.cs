using EmailService.Application.Email.DTOs;
using EmailService.Application.Email.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmailService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmailController : ControllerBase
{
    private readonly ISendEmailService _service;

    public EmailController(ISendEmailService service) => _service = service;

    [HttpPost("send")]
    public async Task<IActionResult> Send([FromBody] SendEmailRequest request, CancellationToken ct)
    {
        await _service.SendAsync(request, ct);
        return Accepted();
    }
}
