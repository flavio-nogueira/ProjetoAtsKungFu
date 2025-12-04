# Sistema de Resposta Padronizada e Logging Completo

## Visão Geral

Este documento descreve o sistema completo de resposta padronizada e logging implementado na API AtsKungFu.

## 📋 Componentes Implementados

### 1. **Resposta Padronizada (`ApiResponse<T>`)**
Todas as respostas da API seguem o formato padronizado:

```json
{
    "sucesso": true,
    "mensagem": "Consulta realizada com sucesso.",
    "dataExecucao": "03/12/2025",
    "horaInicioExecucao": "14:29:10",
    "tempoExecucaoMilisegundos": "116",
    "stackTrace": null,
    "dados": {
        "entidade": {
            // Seus dados aqui
        },
        "sucesso": true,
        "mensagens": [
            {
                "codigo": "Informacao",
                "descricao": "Consulta realizada com sucesso."
            }
        ],
        "status": 200
    }
}
```

### 2. **Middleware de Logging (`RequestResponseLoggingMiddleware`)**
Registra automaticamente:
- ✅ Método HTTP (GET, POST, PUT, DELETE, etc.)
- ✅ Caminho/rota completa
- ✅ QueryString
- ✅ Headers principais (Authorization mascarado)
- ✅ Corpo da requisição (mascarando dados sensíveis)
- ✅ Corpo da resposta
- ✅ IP de origem
- ✅ Usuário autenticado
- ✅ CorrelationId único por requisição
- ✅ Tempo de execução em milissegundos
- ✅ Status code da resposta
- ✅ Tamanho da resposta

### 3. **Middleware de Tratamento de Exceções (`GlobalExceptionHandlerMiddleware`)**
Captura todas as exceções e retorna no formato padronizado da API.

### 4. **Helper de CorrelationId**
Gera ou reutiliza CorrelationId para rastreamento de requisições.

## 🚀 Como Usar nos Controllers

### Exemplo 1: GET - Consulta (Lista)
```csharp
[HttpGet]
public IActionResult GetTodos()
{
    var dados = ObterListaDeDados();

    var response = ApiResponseBuilder<List<MeuTipo>>.Consulta(
        dados,
        "Consulta realizada com sucesso."
    );

    return Ok(response);
}
```

### Exemplo 2: GET por ID - Consulta (Item Único)
```csharp
[HttpGet("{id}")]
public IActionResult GetPorId(Guid id)
{
    var item = _repository.ObterPorId(id);

    if (item == null)
    {
        var errorResponse = ApiResponseBuilder<object>.Erro(
            "Registro não encontrado.",
            404
        );
        return NotFound(errorResponse);
    }

    var response = ApiResponseBuilder<MeuTipo>.Consulta(
        item,
        "Consulta realizada com sucesso."
    );

    return Ok(response);
}
```

### Exemplo 3: POST - Inclusão
```csharp
[HttpPost]
public IActionResult Post([FromBody] MeuRequest request)
{
    if (!ModelState.IsValid)
    {
        var errorResponse = ApiResponseBuilder<object>.Erro(
            "Dados inválidos.",
            400
        );
        return BadRequest(errorResponse);
    }

    var novoRegistro = _service.Criar(request);

    var response = ApiResponseBuilder<MeuTipo>.Inclusao(
        novoRegistro,
        "Registro incluído com sucesso."
    );

    return StatusCode(201, response);
}
```

### Exemplo 4: PUT - Atualização
```csharp
[HttpPut("{id}")]
public IActionResult Put(Guid id, [FromBody] MeuRequest request)
{
    var registroAtualizado = _service.Atualizar(id, request);

    if (registroAtualizado == null)
    {
        var errorResponse = ApiResponseBuilder<object>.Erro(
            "Registro não encontrado.",
            404
        );
        return NotFound(errorResponse);
    }

    var response = ApiResponseBuilder<MeuTipo>.Atualizacao(
        registroAtualizado,
        "Registro atualizado com sucesso."
    );

    return Ok(response);
}
```

### Exemplo 5: DELETE - Exclusão (retorna apenas o ID)
```csharp
[HttpDelete("{id}")]
public IActionResult Delete(Guid id)
{
    var sucesso = _service.Excluir(id);

    if (!sucesso)
    {
        var errorResponse = ApiResponseBuilder<object>.Erro(
            "Registro não encontrado.",
            404
        );
        return NotFound(errorResponse);
    }

    var response = ApiResponseBuilder<Guid>.Exclusao(
        id,
        "Registro excluído com sucesso."
    );

    return Ok(response);
}
```

### Exemplo 6: Usando CorrelationId nos Logs
```csharp
[HttpGet]
public IActionResult MinhaAction()
{
    // Obter o CorrelationId da requisição atual
    var correlationId = HttpContext.GetCorrelationId();

    _logger.LogInformation(
        "Processando requisição. CorrelationId: {CorrelationId}",
        correlationId
    );

    // ... resto do código
}
```

## 📊 Exemplo de Log Estruturado (JSON)

```json
{
    "@t": "2025-12-03T14:29:10.1234567Z",
    "@mt": "Requisição completada | Método: {HttpMethod} | Path: {Path} | QueryString: {QueryString} | StatusCode: {StatusCode} | CorrelationId: {CorrelationId} | Usuario: {Username} | UserId: {UserId} | IP: {IpAddress} | TempoExecucao: {ElapsedMilliseconds}ms | DataHora: {DateTime} | TamanhoResposta: {ResponseSize} bytes | RequestBody: {RequestBody} | ResponseBody: {ResponseBody} | Headers: {Headers}",
    "@l": "Information",
    "HttpMethod": "GET",
    "Path": "/api/exemplo",
    "QueryString": "?filtro=ativo",
    "StatusCode": 200,
    "CorrelationId": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
    "Username": "flavio.nogueira.alfa@outlook.com.br",
    "UserId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
    "IpAddress": "192.168.1.100",
    "ElapsedMilliseconds": 116,
    "DateTime": "2025-12-03 14:29:10.123",
    "ResponseSize": 1024,
    "RequestBody": "",
    "ResponseBody": "{\"sucesso\":true,\"mensagem\":\"Consulta realizada com sucesso.\"...}",
    "Headers": "{\"Content-Type\":\"application/json\",\"Authorization\":\"***MASKED***\"}",
    "Application": "ApiAtsKungFu"
}
```

## 🔒 Segurança e Dados Sensíveis

### Headers Mascarados Automaticamente:
- `Authorization`
- `Cookie`
- `Set-Cookie`
- `X-API-Key`
- `X-Auth-Token`

### Palavras-chave Sensíveis Mascaradas no Payload:
- `password` / `senha`
- `token`
- `secret`
- `cartao` / `card`
- `cvv`
- `pin`

**Exemplo de mascaramento:**
```json
// Request original:
{
    "usuario": "joao@email.com",
    "senha": "123456",
    "token": "abc123"
}

// Log gerado:
{
    "usuario": "joao@email.com",
    "senha": "***MASKED***",
    "token": "***MASKED***"
}
```

## ⚙️ Configuração

### Serilog já está configurado no `appsettings.json`:
```json
{
  "Serilog": {
    "Using": [ "Serilog.Sinks.Console", "Serilog.Sinks.File" ],
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "Microsoft.AspNetCore": "Warning",
        "Microsoft.EntityFrameworkCore": "Information"
      }
    },
    "WriteTo": [
      {
        "Name": "Console"
      },
      {
        "Name": "File",
        "Args": {
          "path": "logs/api-.log",
          "rollingInterval": "Day",
          "rollOnFileSizeLimit": true,
          "fileSizeLimitBytes": 10485760,
          "retainedFileCountLimit": 30
        }
      }
    ],
    "Enrich": [ "FromLogContext", "WithMachineName", "WithThreadId" ],
    "Properties": {
      "Application": "ApiAtsKungFu"
    }
  }
}
```

### Middlewares registrados no `Program.cs` (ordem correta):
```csharp
// 1. Exception Handler - PRIMEIRO para capturar todas as exceções
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

// 2. Request/Response Logging - Logo após exception handler
app.UseMiddleware<RequestResponseLoggingMiddleware>();

// 3. Serilog Request Logging
app.UseSerilogRequestLogging();

// 4. Resto dos middlewares...
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
```

## 📦 Integração com Grafana Loki (Opcional)

Para enviar logs para Grafana Loki, adicione o pacote:
```bash
dotnet add package Serilog.Sinks.Grafana.Loki
```

E configure no `appsettings.json`:
```json
{
  "Serilog": {
    "WriteTo": [
      {
        "Name": "GrafanaLoki",
        "Args": {
          "uri": "http://localhost:3100",
          "labels": [
            {
              "key": "app",
              "value": "ApiAtsKungFu"
            },
            {
              "key": "env",
              "value": "production"
            }
          ]
        }
      }
    ]
  }
}
```

## 🎯 Boas Práticas

1. **Sempre use o `ApiResponseBuilder`** para construir respostas
2. **Nunca retorne objetos diretamente** sem o wrapper de resposta
3. **Use o CorrelationId** nos seus logs personalizados
4. **Não logue dados sensíveis manualmente** - o middleware já faz isso
5. **Configure níveis de log apropriados** para produção (Warning ou Error)
6. **Monitore o tamanho dos logs** - payloads grandes são truncados automaticamente
7. **Use logs estruturados** com placeholders `{NomePropriedade}` em vez de interpolação

## 🔍 Rastreamento de Requisições

Para rastrear uma requisição específica:

1. Cliente envia requisição (opcionalmente pode enviar header `X-Correlation-Id`)
2. API gera ou reutiliza o CorrelationId
3. CorrelationId é incluído em TODOS os logs da requisição
4. CorrelationId é retornado no header da resposta
5. Cliente pode usar o CorrelationId para buscar logs no sistema de log agregado

**Exemplo de busca no Grafana Loki:**
```logql
{app="ApiAtsKungFu"} |= "a1b2c3d4-e5f6-7890-abcd-ef1234567890"
```

## 📝 Testando

Você pode testar os endpoints de exemplo:

```bash
# GET - Listar todos
curl -X GET https://localhost:7000/api/exemplo

# GET - Por ID
curl -X GET https://localhost:7000/api/exemplo/123

# POST - Criar
curl -X POST https://localhost:7000/api/exemplo \
  -H "Content-Type: application/json" \
  -d '{"descricao": "Teste"}'

# PUT - Atualizar
curl -X PUT https://localhost:7000/api/exemplo/123 \
  -H "Content-Type: application/json" \
  -d '{"descricao": "Teste Atualizado"}'

# DELETE - Excluir
curl -X DELETE https://localhost:7000/api/exemplo/123

# Simular erro
curl -X GET https://localhost:7000/api/exemplo/erro
```

## ✅ Checklist de Implementação

- [x] Classes de resposta padronizada criadas
- [x] Builder para facilitar construção de respostas
- [x] Middleware de logging de request/response
- [x] Middleware de tratamento de exceções
- [x] Helper de CorrelationId
- [x] Mascaramento de dados sensíveis
- [x] Truncamento de payloads grandes
- [x] Logging estruturado com Serilog
- [x] Integração no Program.cs
- [x] Controller de exemplo
- [x] Documentação completa

---

**Implementado por:** Claude Code
**Data:** 03/12/2025
**Versão:** 1.0
