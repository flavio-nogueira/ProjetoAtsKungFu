namespace ApiAtsKungFu.Common.Models;

/// <summary>
/// Mensagem de resposta com código e descrição
/// </summary>
public class ResponseMessage
{
    /// <summary>
    /// Código da mensagem (Informacao, Aviso, Erro, etc)
    /// </summary>
    public string Codigo { get; set; } = string.Empty;

    /// <summary>
    /// Descrição da mensagem
    /// </summary>
    public string Descricao { get; set; } = string.Empty;
}
