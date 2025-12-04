namespace ApiAtsKungFu.Common.Models;

/// <summary>
/// Dados internos da resposta
/// </summary>
public class ResponseData<T>
{
    /// <summary>
    /// Entidade ou dados retornados
    /// </summary>
    public T? Entidade { get; set; }

    /// <summary>
    /// Indica sucesso da operação
    /// </summary>
    public bool Sucesso { get; set; }

    /// <summary>
    /// Lista de mensagens retornadas
    /// </summary>
    public List<ResponseMessage> Mensagens { get; set; } = new();

    /// <summary>
    /// Status HTTP da resposta
    /// </summary>
    public int Status { get; set; }
}
