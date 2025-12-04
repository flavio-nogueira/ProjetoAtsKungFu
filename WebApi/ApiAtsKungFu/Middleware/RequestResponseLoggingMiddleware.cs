using ApiAtsKungFu.Common.Extensions;
using Serilog;
using Serilog.Context;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace ApiAtsKungFu.Middleware;

/// <summary>
/// Middleware para logging completo de requisições e respostas
/// Registra: método, rota, query, headers, body, tempo de execução, correlationId, usuário
/// </summary>
public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

    // Limite de tamanho para logar payloads (evitar logs gigantes)
    private const int MaxPayloadSizeToLog = 10000; // 10KB

    // Headers sensíveis que não devem ser logados
    private static readonly HashSet<string> SensitiveHeaders = new(StringComparer.OrdinalIgnoreCase)
    {
        "Authorization",
        "Cookie",
        "Set-Cookie",
        "X-API-Key",
        "X-Auth-Token"
    };

    // Palavras-chave que indicam dados sensíveis no payload
    private static readonly HashSet<string> SensitiveKeywords = new(StringComparer.OrdinalIgnoreCase)
    {
        "password",
        "senha",
        "token",
        "secret",
        "cartao",
        "card",
        "cvv",
        "pin"
    };

    public RequestResponseLoggingMiddleware(
        RequestDelegate next,
        ILogger<RequestResponseLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Gerar ou obter CorrelationId
        var correlationId = context.GetOrCreateCorrelationId();

        // Adicionar CorrelationId ao contexto do Serilog (aparecerá em todos os logs)
        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            // Iniciar cronômetro
            var stopwatch = Stopwatch.StartNew();
            var startTime = DateTime.Now;

            try
            {
                // Ler o body da requisição
                // EnableBuffering permite ler o stream múltiplas vezes
                context.Request.EnableBuffering();
                var requestBody = await ReadRequestBodyAsync(context.Request);

                // Substituir o Response.Body para capturar a resposta
                var originalResponseBodyStream = context.Response.Body;
                using var responseBodyStream = new MemoryStream();
                context.Response.Body = responseBodyStream;

                // Executar o próximo middleware na pipeline
                await _next(context);

                // Parar cronômetro
                stopwatch.Stop();

                // Ler o body da resposta
                responseBodyStream.Seek(0, SeekOrigin.Begin);
                var responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();

                // Copiar a resposta de volta para o stream original
                responseBodyStream.Seek(0, SeekOrigin.Begin);
                await responseBodyStream.CopyToAsync(originalResponseBodyStream);

                // Logar requisição e resposta bem-sucedida
                LogRequestResponse(
                    context,
                    correlationId,
                    requestBody,
                    responseBody,
                    stopwatch.ElapsedMilliseconds,
                    startTime,
                    null);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                // Logar erro
                LogRequestResponse(
                    context,
                    correlationId,
                    string.Empty,
                    string.Empty,
                    stopwatch.ElapsedMilliseconds,
                    startTime,
                    ex);

                // Re-lançar a exceção para o middleware de tratamento de erros
                throw;
            }
        }
    }

    /// <summary>
    /// Lê o body da requisição de forma assíncrona
    /// </summary>
    private async Task<string> ReadRequestBodyAsync(HttpRequest request)
    {
        try
        {
            // Verificar se há conteúdo
            if (request.ContentLength == null || request.ContentLength == 0)
            {
                return string.Empty;
            }

            // Configurar posição do stream para o início
            request.Body.Seek(0, SeekOrigin.Begin);

            // Ler o conteúdo
            using var reader = new StreamReader(
                request.Body,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                bufferSize: 1024,
                leaveOpen: true);

            var body = await reader.ReadToEndAsync();

            // Resetar posição do stream para o início (para próximo middleware poder ler)
            request.Body.Seek(0, SeekOrigin.Begin);

            // Truncar se muito grande
            if (body.Length > MaxPayloadSizeToLog)
            {
                return body[..MaxPayloadSizeToLog] + $"... (truncado, tamanho original: {body.Length} caracteres)";
            }

            // Mascarar dados sensíveis
            return MaskSensitiveData(body);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Erro ao ler body da requisição");
            return string.Empty;
        }
    }

    /// <summary>
    /// Mascara dados sensíveis no payload JSON
    /// </summary>
    private string MaskSensitiveData(string payload)
    {
        if (string.IsNullOrWhiteSpace(payload))
        {
            return payload;
        }

        try
        {
            // Tentar parsear como JSON
            var jsonDocument = JsonDocument.Parse(payload);
            var maskedJson = MaskJsonElement(jsonDocument.RootElement);
            return JsonSerializer.Serialize(maskedJson, new JsonSerializerOptions { WriteIndented = false });
        }
        catch
        {
            // Se não for JSON válido, retornar como está
            return payload;
        }
    }

    /// <summary>
    /// Mascara elementos sensíveis em um JsonElement recursivamente
    /// </summary>
    private object? MaskJsonElement(JsonElement element)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                var obj = new Dictionary<string, object?>();
                foreach (var property in element.EnumerateObject())
                {
                    // Verificar se a propriedade é sensível
                    if (SensitiveKeywords.Any(keyword => property.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
                    {
                        obj[property.Name] = "***MASKED***";
                    }
                    else
                    {
                        obj[property.Name] = MaskJsonElement(property.Value);
                    }
                }
                return obj;

            case JsonValueKind.Array:
                return element.EnumerateArray().Select(MaskJsonElement).ToList();

            case JsonValueKind.String:
                return element.GetString();

            case JsonValueKind.Number:
                return element.TryGetInt64(out var longValue) ? longValue : element.GetDouble();

            case JsonValueKind.True:
                return true;

            case JsonValueKind.False:
                return false;

            case JsonValueKind.Null:
                return null;

            default:
                return element.ToString();
        }
    }

    /// <summary>
    /// Loga todas as informações da requisição e resposta
    /// </summary>
    private void LogRequestResponse(
        HttpContext context,
        string correlationId,
        string requestBody,
        string responseBody,
        long elapsedMilliseconds,
        DateTime startTime,
        Exception? exception)
    {
        var request = context.Request;
        var response = context.Response;

        // Obter usuário autenticado
        var username = context.User?.Identity?.Name ?? "Anonymous";
        var userId = context.User?.FindFirst("sub")?.Value ??
                     context.User?.FindFirst("userId")?.Value ??
                     "N/A";

        // Obter IP de origem
        var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

        // Obter headers principais (mascarar sensíveis)
        var headers = request.Headers
            .Where(h => !SensitiveHeaders.Contains(h.Key))
            .ToDictionary(h => h.Key, h => h.Value.ToString());

        // Adicionar headers sensíveis mascarados
        foreach (var sensitiveHeader in SensitiveHeaders)
        {
            if (request.Headers.ContainsKey(sensitiveHeader))
            {
                headers[sensitiveHeader] = "***MASKED***";
            }
        }

        // Truncar response body se muito grande
        var loggedResponseBody = responseBody;
        if (responseBody.Length > MaxPayloadSizeToLog)
        {
            loggedResponseBody = responseBody[..MaxPayloadSizeToLog] + $"... (truncado, tamanho original: {responseBody.Length} caracteres)";
        }

        if (exception != null)
        {
            // Log de erro estruturado
            _logger.LogError(exception,
                "Requisição falhou | " +
                "Método: {HttpMethod} | " +
                "Path: {Path} | " +
                "QueryString: {QueryString} | " +
                "StatusCode: {StatusCode} | " +
                "CorrelationId: {CorrelationId} | " +
                "Usuario: {Username} | " +
                "UserId: {UserId} | " +
                "IP: {IpAddress} | " +
                "TempoExecucao: {ElapsedMilliseconds}ms | " +
                "DataHora: {DateTime} | " +
                "RequestBody: {RequestBody} | " +
                "Headers: {Headers}",
                request.Method,
                request.Path,
                request.QueryString.ToString(),
                response.StatusCode,
                correlationId,
                username,
                userId,
                ipAddress,
                elapsedMilliseconds,
                startTime.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                requestBody,
                JsonSerializer.Serialize(headers));
        }
        else
        {
            // Log de sucesso estruturado
            _logger.LogInformation(
                "Requisição completada | " +
                "Método: {HttpMethod} | " +
                "Path: {Path} | " +
                "QueryString: {QueryString} | " +
                "StatusCode: {StatusCode} | " +
                "CorrelationId: {CorrelationId} | " +
                "Usuario: {Username} | " +
                "UserId: {UserId} | " +
                "IP: {IpAddress} | " +
                "TempoExecucao: {ElapsedMilliseconds}ms | " +
                "DataHora: {DateTime} | " +
                "TamanhoResposta: {ResponseSize} bytes | " +
                "RequestBody: {RequestBody} | " +
                "ResponseBody: {ResponseBody} | " +
                "Headers: {Headers}",
                request.Method,
                request.Path,
                request.QueryString.ToString(),
                response.StatusCode,
                correlationId,
                username,
                userId,
                ipAddress,
                elapsedMilliseconds,
                startTime.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                responseBody.Length,
                requestBody,
                loggedResponseBody,
                JsonSerializer.Serialize(headers));
        }
    }
}
