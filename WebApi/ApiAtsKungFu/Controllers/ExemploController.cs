using ApiAtsKungFu.Common.Extensions;
using ApiAtsKungFu.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiAtsKungFu.Controllers;

/// <summary>
/// Controller de exemplo demonstrando o uso da resposta padronizada da API
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ExemploController : ControllerBase
{
    private readonly ILogger<ExemploController> _logger;

    public ExemploController(ILogger<ExemploController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Exemplo de consulta (GET) - Lista de items
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<TipoDestino>>), StatusCodes.Status200OK)]
    public IActionResult GetTodos()
    {
        var correlationId = HttpContext.GetCorrelationId();
        _logger.LogInformation("Consultando todos os registros. CorrelationId: {CorrelationId}", correlationId);

        var dados = new List<TipoDestino>
        {
            new() { Codigo = "001", Descricao = "Tipo A" },
            new() { Codigo = "002", Descricao = "Tipo B" },
            new() { Codigo = "003", Descricao = "Tipo C" }
        };

        var response = ApiResponseBuilder<List<TipoDestino>>.Consulta(
            dados,
            "Consulta realizada com sucesso."
        );

        return Ok(response);
    }

    /// <summary>
    /// Exemplo de consulta (GET) por ID - Um único item
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<TipoDestino>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public IActionResult GetPorId(string id)
    {
        var correlationId = HttpContext.GetCorrelationId();
        _logger.LogInformation("Consultando registro por ID: {Id}. CorrelationId: {CorrelationId}", id, correlationId);

        if (id == "999")
        {
            var errorResponse = ApiResponseBuilder<object>.Erro(
                "Registro não encontrado.",
                404
            );
            return NotFound(errorResponse);
        }

        var dados = new TipoDestino { Codigo = id, Descricao = $"Descrição do item {id}" };

        var response = ApiResponseBuilder<TipoDestino>.Consulta(
            dados,
            "Consulta realizada com sucesso."
        );

        return Ok(response);
    }

    /// <summary>
    /// Exemplo de inclusão (POST) - Retorna o objeto criado
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<TipoDestino>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public IActionResult Post([FromBody] TipoDestinoRequest request)
    {
        var correlationId = HttpContext.GetCorrelationId();
        _logger.LogInformation("Criando novo registro. CorrelationId: {CorrelationId}", correlationId);

        if (string.IsNullOrWhiteSpace(request.Descricao))
        {
            var errorResponse = ApiResponseBuilder<object>.Erro(
                "Descrição é obrigatória.",
                400
            );
            return BadRequest(errorResponse);
        }

        // Simular criação
        var novoRegistro = new TipoDestino
        {
            Codigo = Guid.NewGuid().ToString()[..8],
            Descricao = request.Descricao
        };

        var response = ApiResponseBuilder<TipoDestino>.Inclusao(
            novoRegistro,
            "Registro incluído com sucesso."
        );

        return StatusCode(201, response);
    }

    /// <summary>
    /// Exemplo de atualização (PUT) - Retorna o objeto atualizado
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<TipoDestino>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public IActionResult Put(string id, [FromBody] TipoDestinoRequest request)
    {
        var correlationId = HttpContext.GetCorrelationId();
        _logger.LogInformation("Atualizando registro ID: {Id}. CorrelationId: {CorrelationId}", id, correlationId);

        if (id == "999")
        {
            var errorResponse = ApiResponseBuilder<object>.Erro(
                "Registro não encontrado.",
                404
            );
            return NotFound(errorResponse);
        }

        // Simular atualização
        var registroAtualizado = new TipoDestino
        {
            Codigo = id,
            Descricao = request.Descricao
        };

        var response = ApiResponseBuilder<TipoDestino>.Atualizacao(
            registroAtualizado,
            "Registro atualizado com sucesso."
        );

        return Ok(response);
    }

    /// <summary>
    /// Exemplo de exclusão (DELETE) - Retorna apenas o ID excluído
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public IActionResult Delete(string id)
    {
        var correlationId = HttpContext.GetCorrelationId();
        _logger.LogInformation("Excluindo registro ID: {Id}. CorrelationId: {CorrelationId}", id, correlationId);

        if (id == "999")
        {
            var errorResponse = ApiResponseBuilder<object>.Erro(
                "Registro não encontrado.",
                404
            );
            return NotFound(errorResponse);
        }

        // Simular exclusão
        var response = ApiResponseBuilder<string>.Exclusao(
            id,
            "Registro excluído com sucesso."
        );

        return Ok(response);
    }

    /// <summary>
    /// Exemplo de erro - Para testar o tratamento de exceções
    /// </summary>
    [HttpGet("erro")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public IActionResult SimularErro()
    {
        throw new InvalidOperationException("Este é um erro simulado para testar o middleware de exceções.");
    }
}

/// <summary>
/// Modelo de exemplo para demonstração
/// </summary>
public class TipoDestino
{
    public string Codigo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
}

/// <summary>
/// Request de exemplo
/// </summary>
public class TipoDestinoRequest
{
    public string Descricao { get; set; } = string.Empty;
}
