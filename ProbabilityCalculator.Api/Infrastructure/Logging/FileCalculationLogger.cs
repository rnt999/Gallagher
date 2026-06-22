using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace ProbabilityCalculator.Api.Infrastructure.Logging;

public class FileCalculationLogger : ICalculationLogger
{
    private readonly string _logPath;
    private readonly SemaphoreSlim _lock = new(1, 1);

    public FileCalculationLogger(IConfiguration config)
    {
        _logPath = config["LogFilePath"] ?? "logs/calculations.log";
        var dir = Path.GetDirectoryName(_logPath);
        if (!string.IsNullOrEmpty(dir))
            Directory.CreateDirectory(dir);
    }

    public async Task LogAsync<TRequest, TResponse>(
        string operationName, string correlationId, TRequest request, TResponse response)
    {
        var entry = $"{DateTime.UtcNow:O} | {correlationId} | {operationName} | {JsonSerializer.Serialize(request)} | {JsonSerializer.Serialize(response)}{Environment.NewLine}";
        await _lock.WaitAsync();
        try { await File.AppendAllTextAsync(_logPath, entry); }
        finally { _lock.Release(); }
    }
}
