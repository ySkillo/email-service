using System.Collections.Concurrent;
using System.Net;
using System.Text.RegularExpressions;
using EmailService.Application.Email.Interfaces;
using EmailService.Domain.Email.Enums;

namespace EmailService.Infrastructure.Templates;

public sealed partial class HtmlTemplateRenderer : ITemplateRenderer
{
    private static readonly ConcurrentDictionary<string, string> Cache = new();

    [GeneratedRegex(@"\{\{\s*(\w+)\s*\}\}")]
    private static partial Regex PlaceholderRegex();

    public async Task<string> RenderAsync(
        EmailTemplateType type,
        IReadOnlyDictionary<string, string> data,
        CancellationToken cancellationToken = default)
    {
        var fileName = TemplateFileMap.FileFor(type);
        var path = Path.Combine(AppContext.BaseDirectory, "Templates", "Html", fileName);

        if (!Cache.TryGetValue(path, out var html))
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"Template '{fileName}' não encontrado.", path);

            html = await File.ReadAllTextAsync(path, cancellationToken);
            Cache[path] = html;
        }

        return PlaceholderRegex().Replace(html, match =>
        {
            var key = match.Groups[1].Value;

            // Case-insensitive; HtmlEncode evita injeção de HTML pelo valor recebido
            var found = data.FirstOrDefault(d => d.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
            return found.Key is null ? string.Empty : WebUtility.HtmlEncode(found.Value);
        });
    }
}
