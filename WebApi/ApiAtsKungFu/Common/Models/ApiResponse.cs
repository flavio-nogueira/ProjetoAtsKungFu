namespace ApiAtsKungFu.Common.Models;

/// <summary>
/// Modelo de resposta padronizada da API
/// </summary>
public class ApiResponse<T>
{
    /// <summary>
    /// Indica se a operação foi bem-sucedida
    /// </summary>
    public bool Sucesso { get; set; }

    /// <summary>
    /// Mensagem principal da resposta
    /// </summary>
    public string Mensagem { get; set; } = string.Empty;

    /// <summary>
    /// Data de execução da requisição (formato dd/MM/yyyy)
    /// </summary>
    public string DataExecucao { get; set; }

    /// <summary>
    /// Hora de início da execução (formato HH:mm:ss)
    /// </summary>
    public string HoraInicioExecucao { get; set; }

    /// <summary>
    /// Tempo de execução em milissegundos
    /// </summary>
    public string TempoExecucaoMilisegundos { get; set; } = "0";

    /// <summary>
    /// Stack trace em caso de erro (null quando sucesso)
    /// </summary>
    public string? StackTrace { get; set; }

    /// <summary>
    /// Dados da resposta
    /// </summary>
    public ResponseData<T>? Dados { get; set; }

    public ApiResponse()
    {
        var now = DateTime.Now;
        DataExecucao = now.ToString("dd/MM/yyyy");
        HoraInicioExecucao = now.ToString("HH:mm:ss");
    }
}
