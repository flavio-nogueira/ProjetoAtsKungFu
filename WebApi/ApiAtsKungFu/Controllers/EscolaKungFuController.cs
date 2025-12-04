using ApiAtsKungFu.Application.DTOs;
using ApiAtsKungFu.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiAtsKungFu.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de Escolas de Kung Fu (Matrizes e Filiais)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class EscolaKungFuController : ControllerBase
    {
        private readonly IEscolaKungFuService _service;
        private readonly ILogger<EscolaKungFuController> _logger;

        public EscolaKungFuController(
            IEscolaKungFuService service,
            ILogger<EscolaKungFuController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Lista todas as escolas de Kung Fu cadastradas e ativas
        /// </summary>
        /// <remarks>
        /// Retorna uma lista com todas as escolas (matrizes e filiais) que estão ativas no sistema.
        ///
        /// Exemplo de requisição:
        ///
        ///     GET /api/EscolaKungFu
        ///
        /// </remarks>
        /// <returns>Lista de escolas ativas</returns>
        /// <response code="200">Retorna a lista de escolas (pode ser vazia)</response>
        /// <response code="500">Erro interno no servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EscolaKungFuDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<EscolaKungFuDto>>> GetAll()
        {
            try
            {
                var escolas = await _service.ObterTodosAsync();
                return Ok(escolas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar todas as escolas");
                return StatusCode(500, new { message = "Erro interno no servidor" });
            }
        }

        /// <summary>
        /// Busca uma escola específica pelo ID (GUID)
        /// </summary>
        /// <remarks>
        /// Retorna os dados completos de uma escola de Kung Fu identificada pelo GUID.
        ///
        /// Exemplo de requisição:
        ///
        ///     GET /api/EscolaKungFu/550e8400-e29b-41d4-a716-446655440000
        ///
        /// </remarks>
        /// <param name="id">ID único da escola (formato GUID)</param>
        /// <returns>Dados da escola</returns>
        /// <response code="200">Retorna os dados da escola</response>
        /// <response code="404">Escola não encontrada</response>
        /// <response code="500">Erro interno no servidor</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(EscolaKungFuDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EscolaKungFuDto>> GetById(Guid id)
        {
            try
            {
                var escola = await _service.ObterPorIdAsync(id);

                if (escola == null)
                {
                    return NotFound(new { message = "Escola não encontrada" });
                }

                return Ok(escola);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar escola por ID: {Id}", id);
                return StatusCode(500, new { message = "Erro interno no servidor" });
            }
        }

        /// <summary>
        /// Lista apenas as escolas matrizes
        /// </summary>
        /// <remarks>
        /// Retorna apenas as escolas que são matrizes (não inclui filiais).
        ///
        /// Exemplo de requisição:
        ///
        ///     GET /api/EscolaKungFu/matrizes
        ///
        /// </remarks>
        /// <returns>Lista de escolas matrizes</returns>
        /// <response code="200">Retorna a lista de matrizes (pode ser vazia)</response>
        /// <response code="500">Erro interno no servidor</response>
        [HttpGet("matrizes")]
        [ProducesResponseType(typeof(IEnumerable<EscolaKungFuDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<EscolaKungFuDto>>> GetMatrizes()
        {
            try
            {
                var matrizes = await _service.ObterMatrizesAsync();
                return Ok(matrizes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar matrizes");
                return StatusCode(500, new { message = "Erro interno no servidor" });
            }
        }

        /// <summary>
        /// Lista as filiais de uma escola matriz específica
        /// </summary>
        /// <remarks>
        /// Retorna todas as filiais vinculadas a uma escola matriz.
        ///
        /// Exemplo de requisição:
        ///
        ///     GET /api/EscolaKungFu/filiais/550e8400-e29b-41d4-a716-446655440000
        ///
        /// </remarks>
        /// <param name="matrizId">ID da escola matriz (formato GUID)</param>
        /// <returns>Lista de filiais da matriz</returns>
        /// <response code="200">Retorna a lista de filiais (pode ser vazia)</response>
        /// <response code="500">Erro interno no servidor</response>
        [HttpGet("filiais/{matrizId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<EscolaKungFuDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<EscolaKungFuDto>>> GetFiliais(Guid matrizId)
        {
            try
            {
                var filiais = await _service.ObterFiliaisPorMatrizIdAsync(matrizId);
                return Ok(filiais);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar filiais da matriz {MatrizId}", matrizId);
                return StatusCode(500, new { message = "Erro interno no servidor" });
            }
        }

        /// <summary>
        /// Busca uma escola pelo CNPJ
        /// </summary>
        /// <remarks>
        /// Retorna os dados de uma escola identificada pelo CNPJ.
        ///
        /// O CNPJ pode ser informado com ou sem formatação (pontos, barras e hífen).
        ///
        /// Exemplos de requisição:
        ///
        ///     GET /api/EscolaKungFu/cnpj/12.345.678/0001-90
        ///     GET /api/EscolaKungFu/cnpj/12345678000190
        ///
        /// </remarks>
        /// <param name="cnpj">CNPJ da escola (com ou sem formatação)</param>
        /// <returns>Dados da escola</returns>
        /// <response code="200">Retorna os dados da escola</response>
        /// <response code="404">Escola não encontrada com o CNPJ informado</response>
        /// <response code="500">Erro interno no servidor</response>
        [HttpGet("cnpj/{cnpj}")]
        [ProducesResponseType(typeof(EscolaKungFuDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EscolaKungFuDto>> GetByCNPJ(string cnpj)
        {
            try
            {
                var escola = await _service.ObterPorCNPJAsync(cnpj);

                if (escola == null)
                {
                    return NotFound(new { message = "Escola não encontrada com o CNPJ informado" });
                }

                return Ok(escola);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar escola por CNPJ: {CNPJ}", cnpj);
                return StatusCode(500, new { message = "Erro interno no servidor" });
            }
        }

        /// <summary>
        /// Cadastra uma nova escola de Kung Fu (Matriz ou Filial)
        /// </summary>
        /// <remarks>
        /// Cria um novo cadastro de escola de Kung Fu no sistema.
        ///
        /// **Campos obrigatórios:**
        /// - tipo: "Matriz" ou "Filial"
        /// - cnpj: CNPJ único (não pode estar cadastrado)
        /// - razaoSocial: Razão social da empresa
        /// - logradouro, numero, bairro, cidade, uf, cep: Endereço completo
        /// - idUsuarioCadastrou: GUID do usuário que está criando o cadastro
        ///
        /// **Para Filiais:**
        /// - idEmpresaMatriz: GUID da escola matriz (obrigatório)
        ///
        /// Exemplo de requisição (Matriz):
        ///
        ///     POST /api/EscolaKungFu
        ///     {
        ///        "tipo": "Matriz",
        ///        "cnpj": "12.345.678/0001-90",
        ///        "razaoSocial": "Academia de Kung Fu LTDA",
        ///        "nomeFantasia": "Academia Master",
        ///        "logradouro": "Rua das Artes Marciais",
        ///        "numero": "100",
        ///        "bairro": "Centro",
        ///        "cidade": "São Paulo",
        ///        "uf": "SP",
        ///        "cep": "01234-567",
        ///        "email": "contato@academia.com",
        ///        "celularWhatsApp": "(11) 98765-4321",
        ///        "idUsuarioCadastrou": "660e8400-e29b-41d4-a716-446655440000"
        ///     }
        ///
        /// Exemplo de requisição (Filial):
        ///
        ///     POST /api/EscolaKungFu
        ///     {
        ///        "tipo": "Filial",
        ///        "cnpj": "12.345.678/0002-71",
        ///        "razaoSocial": "Academia de Kung Fu LTDA",
        ///        "nomeFantasia": "Academia Master - Filial Centro",
        ///        "idEmpresaMatriz": "550e8400-e29b-41d4-a716-446655440000",
        ///        "logradouro": "Av. Paulista",
        ///        "numero": "1500",
        ///        "bairro": "Bela Vista",
        ///        "cidade": "São Paulo",
        ///        "uf": "SP",
        ///        "cep": "01310-100",
        ///        "idUsuarioCadastrou": "660e8400-e29b-41d4-a716-446655440000"
        ///     }
        ///
        /// </remarks>
        /// <param name="createDto">Dados da escola a ser criada</param>
        /// <returns>Escola criada com ID gerado</returns>
        /// <response code="201">Escola criada com sucesso</response>
        /// <response code="400">Dados inválidos ou CNPJ já cadastrado</response>
        /// <response code="500">Erro interno no servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(EscolaKungFuDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EscolaKungFuDto>> Create([FromBody] CreateEscolaKungFuDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var escola = await _service.IncluirAsync(createDto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = escola.Id },
                    escola);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Operação inválida ao criar escola");
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Argumento inválido ao criar escola");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar escola");
                return StatusCode(500, new { message = "Erro interno no servidor" });
            }
        }

        /// <summary>
        /// Atualiza os dados de uma escola existente
        /// </summary>
        /// <remarks>
        /// Atualiza as informações cadastrais de uma escola.
        ///
        /// **Importante:**
        /// - O ID da escola não pode ser alterado
        /// - O CNPJ não pode ser alterado
        /// - O tipo (Matriz/Filial) não pode ser alterado
        /// - É obrigatório informar o idUsuarioAlterou
        ///
        /// **Campos obrigatórios:**
        /// - razaoSocial
        /// - logradouro, numero, bairro, cidade, uf, cep
        /// - idUsuarioAlterou: GUID do usuário que está fazendo a alteração
        ///
        /// Exemplo de requisição:
        ///
        ///     PUT /api/EscolaKungFu/550e8400-e29b-41d4-a716-446655440000
        ///     {
        ///        "razaoSocial": "Academia de Kung Fu LTDA - Atualizada",
        ///        "nomeFantasia": "Academia Master Premium",
        ///        "logradouro": "Rua das Artes Marciais",
        ///        "numero": "100-A",
        ///        "bairro": "Centro",
        ///        "cidade": "São Paulo",
        ///        "uf": "SP",
        ///        "cep": "01234-567",
        ///        "pais": "Brasil",
        ///        "email": "novo@academia.com",
        ///        "celularWhatsApp": "(11) 98765-4321",
        ///        "idUsuarioAlterou": "660e8400-e29b-41d4-a716-446655440001"
        ///     }
        ///
        /// </remarks>
        /// <param name="id">ID da escola a ser atualizada (GUID)</param>
        /// <param name="updateDto">Dados atualizados da escola</param>
        /// <returns>Escola atualizada</returns>
        /// <response code="200">Escola atualizada com sucesso</response>
        /// <response code="400">Dados inválidos</response>
        /// <response code="404">Escola não encontrada</response>
        /// <response code="500">Erro interno no servidor</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(EscolaKungFuDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EscolaKungFuDto>> Update(Guid id, [FromBody] UpdateEscolaKungFuDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var escola = await _service.AlterarAsync(id, updateDto);

                return Ok(escola);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Operação inválida ao atualizar escola {Id}", id);

                if (ex.Message.Contains("não encontrada"))
                {
                    return NotFound(new { message = ex.Message });
                }

                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Argumento inválido ao atualizar escola {Id}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar escola {Id}", id);
                return StatusCode(500, new { message = "Erro interno no servidor" });
            }
        }

        /// <summary>
        /// Desativa uma escola (exclusão lógica/soft delete)
        /// </summary>
        /// <remarks>
        /// Remove uma escola do cadastro ativo através de exclusão lógica (soft delete).
        ///
        /// **Importante:**
        /// - A escola não é deletada fisicamente do banco de dados
        /// - Apenas o campo "Ativo" é marcado como false
        /// - Os dados permanecem no banco para fins de auditoria
        /// - Escolas desativadas não aparecem nas listagens normais
        ///
        /// Exemplo de requisição:
        ///
        ///     DELETE /api/EscolaKungFu/550e8400-e29b-41d4-a716-446655440000
        ///
        /// </remarks>
        /// <param name="id">ID da escola a ser desativada (GUID)</param>
        /// <returns>Sem conteúdo</returns>
        /// <response code="204">Escola desativada com sucesso</response>
        /// <response code="404">Escola não encontrada</response>
        /// <response code="500">Erro interno no servidor</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _service.ExcluirAsync(id);

                if (!result)
                {
                    return NotFound(new { message = "Escola não encontrada" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar escola {Id}", id);
                return StatusCode(500, new { message = "Erro interno no servidor" });
            }
        }
    }
}
