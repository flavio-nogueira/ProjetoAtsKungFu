using ApiAtsKungFu.Common.Extensions;
using ApiAtsKungFu.Common.Models;
using System.Net;
using System.Text.Json;

namespace ApiAtsKungFu.Middleware;

/// <summary>
/// Middleware para tratamento global de exceções
/// Garante que todas as exceções retornem no formato padronizado da API
/// </summary>
public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public GlobalExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlerMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exceção não tratada capturada pelo GlobalExceptionHandler");
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var correlationId = context.GetCorrelationId();

        // Determinar status code baseado no tipo de exceção
        var (statusCode, message) = exception switch
        {
            ArgumentException => (HttpStatusCode.BadRequest, exception.Message),
            KeyNotFoundException => (HttpStatusCode.NotFound, "Recurso não encontrado."),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Não autorizado."),
            InvalidOperationException => (HttpStatusCode.BadRequest, exception.Message),
            _ => (HttpStatusCode.InternalServerError, "Erro interno do servidor.")
        };

        // Em produção, não expor detalhes internos
        var errorMessage = _environment.IsDevelopment()
            ? exception.Message
            : message;

        // Stack trace apenas em desenvolvimento
        var stackTrace = _environment.IsDevelopment()
            ? exception.StackTrace
            : null;

        // Criar resposta padronizada de erro
        var response = new ApiResponse<object>
        {
            Sucesso = false,
            Mensagem = errorMessage,
            StackTrace = stackTrace,
            Dados = new ResponseData<object>
            {
                Sucesso = false,
                Status = (int)statusCode,
                Mensagens = new List<ResponseMessage>
                {
                    new()
                    {
                        Codigo = "Erro",
                        Descricao = errorMessage
                    }
                }
            }
        };

        // Incluir inner exception em desenvolvimento
        if (_environment.IsDevelopment() && exception.InnerException != null)
        {
            response.Dados.Mensagens.Add(new ResponseMessage
            {
                Codigo = "InnerException",
                Descricao = exception.InnerException.Message
            });
        }

        // Configurar resposta HTTP
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        // Adicionar CorrelationId ao header da resposta
        if (!context.Response.Headers.ContainsKey("X-Correlation-Id"))
        {
            context.Response.Headers.Append("X-Correlation-Id", correlationId);
        }

        // Serializar e enviar resposta
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = _environment.IsDevelopment()
        };

        var json = JsonSerializer.Serialize(response, options);
        await context.Response.WriteAsync(json);
    }
}
