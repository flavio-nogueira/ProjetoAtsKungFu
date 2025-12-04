namespace ApiAtsKungFu.Common.Extensions;

/// <summary>
/// Extensions para gerenciar CorrelationId nas requisições
/// </summary>
public static class CorrelationIdExtensions
{
    private const string CorrelationIdKey = "CorrelationId";
    private const string CorrelationIdHeader = "X-Correlation-Id";

    /// <summary>
    /// Obtém o CorrelationId da requisição atual
    /// </summary>
    public static string GetCorrelationId(this HttpContext context)
    {
        if (context.Items.TryGetValue(CorrelationIdKey, out var correlationId) && correlationId != null)
        {
            return correlationId.ToString()!;
        }
        return string.Empty;
    }

    /// <summary>
    /// Define o CorrelationId na requisição atual
    /// </summary>
    public static void SetCorrelationId(this HttpContext context, string correlationId)
    {
        context.Items[CorrelationIdKey] = correlationId;

        // Adiciona também no header da resposta para o cliente poder rastrear
        if (!context.Response.Headers.ContainsKey(CorrelationIdHeader))
        {
            context.Response.Headers.Append(CorrelationIdHeader, correlationId);
        }
    }

    /// <summary>
    /// Obtém ou gera um novo CorrelationId
    /// </summary>
    public static string GetOrCreateCorrelationId(this HttpContext context)
    {
        // Tenta obter do header da requisição
        if (context.Request.Headers.TryGetValue(CorrelationIdHeader, out var correlationIdFromHeader)
            && !string.IsNullOrWhiteSpace(correlationIdFromHeader))
        {
            var correlationId = correlationIdFromHeader.ToString();
            context.SetCorrelationId(correlationId);
            return correlationId;
        }

        // Tenta obter do contexto
        var existingId = context.GetCorrelationId();
        if (!string.IsNullOrEmpty(existingId))
        {
            return existingId;
        }

        // Gera um novo
        var newCorrelationId = Guid.NewGuid().ToString();
        context.SetCorrelationId(newCorrelationId);
        return newCorrelationId;
    }
}
