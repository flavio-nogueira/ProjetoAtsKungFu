using System.Diagnostics;

namespace ApiAtsKungFu.Common.Models;

/// <summary>
/// Builder para construir respostas padronizadas da API
/// </summary>
public class ApiResponseBuilder<T>
{
    private readonly ApiResponse<T> _response;
    private readonly Stopwatch _stopwatch;
    private readonly DateTime _startTime;

    public ApiResponseBuilder()
    {
        _response = new ApiResponse<T>();
        _stopwatch = Stopwatch.StartNew();
        _startTime = DateTime.Now;
        _response.DataExecucao = _startTime.ToString("dd/MM/yyyy");
        _response.HoraInicioExecucao = _startTime.ToString("HH:mm:ss");
    }

    /// <summary>
    /// Define o sucesso da operação
    /// </summary>
    public ApiResponseBuilder<T> ComSucesso(bool sucesso)
    {
        _response.Sucesso = sucesso;
        if (_response.Dados != null)
        {
            _response.Dados.Sucesso = sucesso;
        }
        return this;
    }

    /// <summary>
    /// Define a mensagem principal
    /// </summary>
    public ApiResponseBuilder<T> ComMensagem(string mensagem)
    {
        _response.Mensagem = mensagem;
        return this;
    }

    /// <summary>
    /// Define os dados da entidade
    /// </summary>
    public ApiResponseBuilder<T> ComDados(T dados)
    {
        if (_response.Dados == null)
        {
            _response.Dados = new ResponseData<T>();
        }
        _response.Dados.Entidade = dados;
        return this;
    }

    /// <summary>
    /// Define o status HTTP
    /// </summary>
    public ApiResponseBuilder<T> ComStatus(int status)
    {
        if (_response.Dados == null)
        {
            _response.Dados = new ResponseData<T>();
        }
        _response.Dados.Status = status;
        return this;
    }

    /// <summary>
    /// Adiciona uma mensagem à lista de mensagens
    /// </summary>
    public ApiResponseBuilder<T> AdicionarMensagem(string codigo, string descricao)
    {
        if (_response.Dados == null)
        {
            _response.Dados = new ResponseData<T>();
        }
        _response.Dados.Mensagens.Add(new ResponseMessage
        {
            Codigo = codigo,
            Descricao = descricao
        });
        return this;
    }

    /// <summary>
    /// Define o stack trace em caso de erro
    /// </summary>
    public ApiResponseBuilder<T> ComStackTrace(string? stackTrace)
    {
        _response.StackTrace = stackTrace;
        return this;
    }

    /// <summary>
    /// Constrói a resposta final com o tempo de execução calculado
    /// </summary>
    public ApiResponse<T> Build()
    {
        _stopwatch.Stop();
        _response.TempoExecucaoMilisegundos = _stopwatch.ElapsedMilliseconds.ToString();

        // Garantir que Dados existe
        if (_response.Dados == null)
        {
            _response.Dados = new ResponseData<T>
            {
                Sucesso = _response.Sucesso,
                Status = 200
            };
        }

        // Sincronizar sucesso
        _response.Dados.Sucesso = _response.Sucesso;

        return _response;
    }

    /// <summary>
    /// Cria uma resposta de sucesso padrão
    /// </summary>
    public static ApiResponse<T> Sucesso(T dados, string mensagem = "Operação realizada com sucesso.", int status = 200)
    {
        return new ApiResponseBuilder<T>()
            .ComSucesso(true)
            .ComMensagem(mensagem)
            .ComDados(dados)
            .ComStatus(status)
            .AdicionarMensagem("Informacao", mensagem)
            .Build();
    }

    /// <summary>
    /// Cria uma resposta de erro padrão
    /// </summary>
    public static ApiResponse<T> Erro(string mensagem, int status = 400, string? stackTrace = null)
    {
        return new ApiResponseBuilder<T>()
            .ComSucesso(false)
            .ComMensagem(mensagem)
            .ComStatus(status)
            .ComStackTrace(stackTrace)
            .AdicionarMensagem("Erro", mensagem)
            .Build();
    }

    /// <summary>
    /// Cria uma resposta para consulta bem-sucedida
    /// </summary>
    public static ApiResponse<T> Consulta(T dados, string mensagem = "Consulta realizada com sucesso.")
    {
        return Sucesso(dados, mensagem, 200);
    }

    /// <summary>
    /// Cria uma resposta para inclusão bem-sucedida
    /// </summary>
    public static ApiResponse<T> Inclusao(T dados, string mensagem = "Registro incluído com sucesso.")
    {
        return Sucesso(dados, mensagem, 201);
    }

    /// <summary>
    /// Cria uma resposta para atualização bem-sucedida
    /// </summary>
    public static ApiResponse<T> Atualizacao(T dados, string mensagem = "Registro atualizado com sucesso.")
    {
        return Sucesso(dados, mensagem, 200);
    }

    /// <summary>
    /// Cria uma resposta para exclusão bem-sucedida (retorna apenas o ID)
    /// </summary>
    public static ApiResponse<T> Exclusao(T id, string mensagem = "Registro excluído com sucesso.")
    {
        return Sucesso(id, mensagem, 200);
    }
}
