# API AtsKungFu - Documentação de Configuração

## 📋 Resumo das Alterações Implementadas

### ✅ 1. Migração para GUID
- Todos os IDs foram alterados de `int` para `Guid`
- Campos de auditoria adicionados:
  - `IdUsuarioCadastrou` (Guid, obrigatório)
  - `IdUsuarioAlterou` (Guid, nullable)
  - `CadastroAtivo` (bool, padrão true)

### ✅ 2. Métodos em Português
Todos os métodos das interfaces e implementações foram traduzidos:
- `GetByIdAsync` → `ObterPorIdAsync`
- `GetAllAsync` → `ObterTodosAsync`
- `CreateAsync` → `IncluirAsync`
- `UpdateAsync` → `AlterarAsync`
- `DeleteAsync` → `ExcluirAsync`
- `AddAsync` → `IncluirAsync`
- `SaveAsync` → `SalvarAsync`

### ✅ 3. Serilog Configurado
- Logs gravados em Console e Arquivo
- Arquivos de log rotativos diários em `logs/api-.log`
- Limite de 10MB por arquivo
- Retenção de 30 dias

### ✅ 4. Prometheus Configurado
- Métricas HTTP automáticas
- Endpoint: `/metrics`

### ✅ 5. Health Checks Configurados
- Verificação de saúde do MySQL
- Endpoints:
  - `/health` - Status geral
  - `/health/ready` - Pronto para receber requisições
  - `/health/live` - Aplicação está viva

---

## 🗄️ PASSO 1: Aplicar Migration no Banco de Dados

**IMPORTANTE**: Execute o script SQL antes de iniciar a API!

### Opção A: MySQL Workbench

1. Abra o MySQL Workbench
2. Conecte ao servidor:
   - Host: `168.231.95.240`
   - Port: `3306`
   - Username: `root` ou usuário com privilégios admin
3. Abra o arquivo: `APLICAR_MIGRATION_GUID.sql`
4. Execute o script (clique no ícone de raio ou F5)
5. Verifique se apareceu: "Tabela recriada com sucesso com GUID!"

### Opção B: Linha de Comando MySQL

```bash
mysql -h 168.231.95.240 -u root -p < APLICAR_MIGRATION_GUID.sql
```

### O que o script faz:
1. Remove a Foreign Key antiga
2. Dropa a tabela `EscolaKungFu` (⚠️ APAGA TODOS OS DADOS)
3. Remove migration antiga do histórico
4. Cria nova tabela com campos GUID
5. Cria índices otimizados
6. Registra ambas migrations no histórico

---

## 🚀 PASSO 2: Iniciar a API

```bash
cd C:\Desenvolvimento\ProjetoAtsKungFu\WebApi\ApiAtsKungFu
dotnet run
```

A API iniciará em:
- HTTPS: https://localhost:7073
- HTTP: http://localhost:5099

---

## 🧪 PASSO 3: Testar a API

### 1. Swagger UI
Acesse: https://localhost:7073/swagger

### 2. Health Checks

```bash
# Status geral
curl http://localhost:5099/health

# Verificar se está pronta
curl http://localhost:5099/health/ready

# Verificar se está viva
curl http://localhost:5099/health/live
```

Resposta esperada:
```json
{"status":"Healthy","totalDuration":"00:00:00.1234567"}
```

### 3. Métricas do Prometheus

```bash
curl http://localhost:5099/metrics
```

Você verá métricas como:
```
# HELP http_requests_received_total Total number of HTTP requests received
# TYPE http_requests_received_total counter
http_requests_received_total{code="200",method="GET",controller="EscolaKungFu",action="GetAll"} 5

# HELP http_request_duration_seconds HTTP request duration in seconds
# TYPE http_request_duration_seconds histogram
http_request_duration_seconds_sum 0.523
```

### 4. Criar uma Escola (Exemplo)

**Endpoint**: POST `/api/EscolaKungFu`

**Body**:
```json
{
  "tipo": "Matriz",
  "cnpj": "12.345.678/0001-90",
  "razaoSocial": "Academia de Kung Fu LTDA",
  "nomeFantasia": "Academia Master",
  "logradouro": "Rua das Artes Marciais",
  "numero": "100",
  "bairro": "Centro",
  "cidade": "São Paulo",
  "uf": "SP",
  "cep": "01234-567",
  "email": "contato@academiamaster.com.br",
  "celularWhatsApp": "(11) 98765-4321",
  "idUsuarioCadastrou": "550e8400-e29b-41d4-a716-446655440000"
}
```

**IMPORTANTE**: O campo `idUsuarioCadastrou` é obrigatório e deve ser um GUID válido.

### 5. Atualizar uma Escola

**Endpoint**: PUT `/api/EscolaKungFu/{id}`

**Body**:
```json
{
  "razaoSocial": "Academia de Kung Fu LTDA - Atualizada",
  "nomeFantasia": "Academia Master Premium",
  "logradouro": "Rua das Artes Marciais",
  "numero": "100",
  "bairro": "Centro",
  "cidade": "São Paulo",
  "uf": "SP",
  "cep": "01234-567",
  "pais": "Brasil",
  "email": "novo@academiamaster.com.br",
  "celularWhatsApp": "(11) 98765-4321",
  "idUsuarioAlterou": "550e8400-e29b-41d4-a716-446655440001"
}
```

**IMPORTANTE**: O campo `idUsuarioAlterou` é obrigatório.

---

## 📊 PASSO 4: Visualizar Logs

### Logs em Console
Os logs aparecem automaticamente no terminal onde você executou `dotnet run`.

Formato:
```
[14:23:45 INF] Iniciando API AtsKungFu
[14:23:46 INF] API AtsKungFu iniciada com sucesso
[14:23:50 INF] HTTP GET /api/EscolaKungFu responded 200 in 45.2314 ms
```

### Logs em Arquivo

Os logs são salvos em:
```
C:\Desenvolvimento\ProjetoAtsKungFu\WebApi\ApiAtsKungFu\logs\
```

Arquivos:
- `api-20251202.log` - Log de hoje
- `api-20251201.log` - Log de ontem
- etc...

Formato do arquivo:
```
2025-12-02 14:23:45.123 -03:00 [INF] Iniciando API AtsKungFu
2025-12-02 14:23:46.456 -03:00 [INF] API AtsKungFu iniciada com sucesso
2025-12-02 14:23:50.789 -03:00 [INF] HTTP GET /api/EscolaKungFu responded 200 in 45.2314 ms
```

---

## 🔍 Monitoramento com Prometheus

### Configurar Prometheus (Opcional)

1. Baixe o Prometheus: https://prometheus.io/download/
2. Crie um arquivo `prometheus.yml`:

```yaml
global:
  scrape_interval: 15s

scrape_configs:
  - job_name: 'api-atskungfu'
    static_configs:
      - targets: ['localhost:5099']
```

3. Execute o Prometheus:
```bash
prometheus --config.file=prometheus.yml
```

4. Acesse: http://localhost:9090

### Grafana (Opcional)

1. Baixe o Grafana: https://grafana.com/grafana/download
2. Adicione Prometheus como data source
3. Crie dashboards personalizados

---

## 📝 Endpoints Disponíveis

### Escolas de Kung Fu

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/EscolaKungFu` | Listar todas as escolas ativas |
| GET | `/api/EscolaKungFu/{id}` | Buscar escola por ID (GUID) |
| GET | `/api/EscolaKungFu/cnpj/{cnpj}` | Buscar por CNPJ |
| GET | `/api/EscolaKungFu/matrizes` | Listar apenas matrizes |
| GET | `/api/EscolaKungFu/filiais/{matrizId}` | Listar filiais de uma matriz |
| POST | `/api/EscolaKungFu` | Criar nova escola |
| PUT | `/api/EscolaKungFu/{id}` | Atualizar escola |
| DELETE | `/api/EscolaKungFu/{id}` | Desativar escola (soft delete) |

### Observabilidade

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/health` | Status geral da aplicação |
| GET | `/health/ready` | Verifica se está pronta |
| GET | `/health/live` | Verifica se está viva |
| GET | `/metrics` | Métricas do Prometheus |
| GET | `/swagger` | Documentação Swagger |

---

## ⚠️ Pontos de Atenção

### 1. Campos Obrigatórios de Auditoria

Ao criar ou atualizar escolas, **sempre** forneça:
- `idUsuarioCadastrou` (ao criar)
- `idUsuarioAlterou` (ao atualizar)

### 2. Formato de GUID

Os GUIDs devem estar no formato:
```
550e8400-e29b-41d4-a716-446655440000
```

### 3. Migration do Banco

**⚠️ CUIDADO**: O script SQL apaga todos os dados da tabela `EscolaKungFu`!

Se você já tem dados importantes, faça backup antes:
```sql
CREATE TABLE EscolaKungFu_Backup AS SELECT * FROM EscolaKungFu;
```

### 4. Permissões MySQL

O usuário `atskungfu` precisa ter permissões para:
- CREATE TABLE
- DROP TABLE
- ALTER TABLE
- INSERT, UPDATE, DELETE

---

## 🐛 Troubleshooting

### Erro: "Tabela EscolaKungFu não existe"
- Execute o script SQL `APLICAR_MIGRATION_GUID.sql`

### Erro: "Health check failed"
- Verifique se o MySQL está rodando
- Verifique se a connection string está correta
- Verifique se o banco `dbatskungfu` existe

### Erro: "Access denied for user 'atskungfu'"
- Execute o script com usuário root
- Verifique as permissões do usuário

### Logs não aparecem em arquivo
- Verifique se a pasta `logs` foi criada
- Verifique permissões de escrita

---

## 📦 Pacotes NuGet Instalados

- **Pomelo.EntityFrameworkCore.MySql** - Provider MySQL para EF Core
- **AutoMapper** - Mapeamento DTO ↔ Entidade
- **Serilog.AspNetCore** - Logging estruturado
- **Serilog.Sinks.Console** - Logs no console
- **Serilog.Sinks.File** - Logs em arquivo
- **prometheus-net.AspNetCore** - Métricas Prometheus
- **AspNetCore.HealthChecks.MySql** - Health check MySQL

---

## 🎯 Próximos Passos Sugeridos

1. ✅ Aplicar migration no banco
2. ✅ Testar API via Swagger
3. ✅ Verificar Health Checks
4. ✅ Verificar Logs
5. ⬜ Implementar autenticação/autorização
6. ⬜ Adicionar validações customizadas
7. ⬜ Implementar cache com Redis
8. ⬜ Configurar CI/CD

---

## 📞 Suporte

Em caso de dúvidas ou problemas:
1. Verifique os logs em `logs/api-*.log`
2. Verifique o Health Check: `/health`
3. Consulte esta documentação

---

**Última atualização**: 02/12/2025
**Versão da API**: 1.0.0
