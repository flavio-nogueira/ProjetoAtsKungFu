using System.Diagnostics;

namespace ApiAtsKungFu.Application.DTOs.Common
{
    /// <summary>
    /// Resposta padrão da API
    /// </summary>
    public class ApiResponse<T>
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public string DataExecucao { get; set; }
        public string HoraInicioExecucao { get; set; }
        public string TempoExecucaoMilisegundos { get; set; }
        public string? StackTrace { get; set; }
        public DadosResponse<T>? Dados { get; set; }

        public ApiResponse()
        {
            var now = DateTime.Now;
            DataExecucao = now.ToString("dd/MM/yyyy");
            HoraInicioExecucao = now.ToString("HH:mm:ss");
        }

        /// <summary>
        /// Cria uma resposta de sucesso
        /// </summary>
        public static ApiResponse<T> Success(T? entidade, string mensagem = "Operação realizada com sucesso", int status = 200, long? tempoExecucaoMs = null)
        {
            var response = new ApiResponse<T>
            {
                Sucesso = true,
                Mensagem = mensagem,
                TempoExecucaoMilisegundos = tempoExecucaoMs?.ToString() ?? "0",
                Dados = new DadosResponse<T>
                {
                    Entidade = entidade,
                    Sucesso = true,
                    Mensagens = new List<MensagemResponse>
                    {
                        new MensagemResponse
                        {
                            Codigo = "Informacao",
                            Descricao = mensagem
                        }
                    },
                    Status = status
                }
            };

            return response;
        }

        /// <summary>
        /// Cria uma resposta de erro
        /// </summary>
        public static ApiResponse<T> Error(string mensagem, int status = 400, string? stackTrace = null, long? tempoExecucaoMs = null)
        {
            var response = new ApiResponse<T>
            {
                Sucesso = false,
                Mensagem = mensagem,
                StackTrace = stackTrace,
                TempoExecucaoMilisegundos = tempoExecucaoMs?.ToString() ?? "0",
                Dados = new DadosResponse<T>
                {
                    Entidade = default,
                    Sucesso = false,
                    Mensagens = new List<MensagemResponse>
                    {
                        new MensagemResponse
                        {
                            Codigo = "Erro",
                            Descricao = mensagem
                        }
                    },
                    Status = status
                }
            };

            return response;
        }
    }

    /// <summary>
    /// Dados da resposta
    /// </summary>
    public class DadosResponse<T>
    {
        public T? Entidade { get; set; }
        public bool Sucesso { get; set; }
        public List<MensagemResponse> Mensagens { get; set; } = new();
        public int Status { get; set; }
    }

    /// <summary>
    /// Mensagem de retorno
    /// </summary>
    public class MensagemResponse
    {
        public string Codigo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
    }
}
