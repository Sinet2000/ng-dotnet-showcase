using Webstore.API.Application.Common.Models;

namespace Webstore.API.Application.Common.DTOs;

public class ApiErrorDetailsDto
{
    public string Message { get; set; } = string.Empty;
    public Dictionary<string, string[]>? ValidationErrors { get; set; }
    public string? TraceId { get; set; }

    public static ApiErrorDetailsDto Create(string message, Dictionary<string, string[]>? validationErrors = null)
        => new() { Message = message, ValidationErrors = validationErrors };

    public static ApiErrorDetailsDto FromResult(Result result)
        => new() { Message = string.Join("; ", result.Errors) };
}