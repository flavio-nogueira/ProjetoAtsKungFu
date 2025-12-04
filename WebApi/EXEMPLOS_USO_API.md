# 📚 Exemplos de Uso da API AtsKungFu

## 🔗 Base URL
```
HTTPS: https://localhost:7073
HTTP:  http://localhost:5099
```

---

## 1️⃣ GET /api/EscolaKungFu
**Descrição**: Listar todas as escolas de Kung Fu cadastradas e ativas.

### Request
```http
GET /api/EscolaKungFu HTTP/1.1
Host: localhost:5099
Accept: application/json
```

### cURL
```bash
curl -X GET "http://localhost:5099/api/EscolaKungFu" -H "accept: application/json"
```

### Response 200 OK
```json
[
  {
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "tipo": "Matriz",
    "eMatriz": true,
    "cnpj": "12.345.678/0001-90",
    "razaoSocial": "Academia de Kung Fu LTDA",
    "nomeFantasia": "Academia Master",
    "inscricaoEstadual": "123.456.789.012",
    "inscricaoMunicipal": "987654",
    "cnaePrincipal": "8591-1/00",
    "cnaeSecundarios": null,
    "regimeTributario": "Simples Nacional",
    "logradouro": "Rua das Artes Marciais",
    "numero": "100",
    "complemento": "Sala 201",
    "bairro": "Centro",
    "cidade": "São Paulo",
    "uf": "SP",
    "cep": "01234-567",
    "pais": "Brasil",
    "telefoneFixo": "(11) 3333-4444",
    "celularWhatsApp": "(11) 98765-4321",
    "email": "contato@academiamaster.com.br",
    "site": "https://www.academiamaster.com.br",
    "nomeResponsavel": "João Silva",
    "quantidadeFiliais": 3,
    "inscricoesAutorizacoes": "CREF 123456",
    "idEmpresaMatriz": null,
    "codigoFilial": null,
    "dataCriacao": "2025-12-02T10:30:00",
    "dataAlteracao": null,
    "ativo": true,
    "cadastroAtivo": true,
    "idUsuarioCadastrou": "660e8400-e29b-41d4-a716-446655440000",
    "idUsuarioAlterou": null
  },
  {
    "id": "550e8400-e29b-41d4-a716-446655440001",
    "tipo": "Filial",
    "eMatriz": false,
    "cnpj": "12.345.678/0002-71",
    "razaoSocial": "Academia de Kung Fu LTDA",
    "nomeFantasia": "Academia Master - Filial Centro",
    "inscricaoEstadual": null,
    "inscricaoMunicipal": null,
    "cnaePrincipal": null,
    "cnaeSecundarios": null,
    "regimeTributario": null,
    "logradouro": "Avenida Paulista",
    "numero": "1500",
    "complemento": "Loja 5",
    "bairro": "Bela Vista",
    "cidade": "São Paulo",
    "uf": "SP",
    "cep": "01310-100",
    "pais": "Brasil",
    "telefoneFixo": null,
    "celularWhatsApp": "(11) 97777-8888",
    "email": "filialcentro@academiamaster.com.br",
    "site": null,
    "nomeResponsavel": "Maria Santos",
    "quantidadeFiliais": null,
    "inscricoesAutorizacoes": null,
    "idEmpresaMatriz": "550e8400-e29b-41d4-a716-446655440000",
    "codigoFilial": "FIL-001",
    "dataCriacao": "2025-12-02T11:00:00",
    "dataAlteracao": null,
    "ativo": true,
    "cadastroAtivo": true,
    "idUsuarioCadastrou": "660e8400-e29b-41d4-a716-446655440000",
    "idUsuarioAlterou": null
  }
]
```

### Response 200 OK (Lista Vazia)
```json
[]
```

### Response 500 Internal Server Error
```json
{
  "message": "Erro interno no servidor"
}
```

---

## 2️⃣ POST /api/EscolaKungFu
**Descrição**: Cadastrar uma nova escola de Kung Fu (Matriz ou Filial).

### Request - Criar Matriz
```http
POST /api/EscolaKungFu HTTP/1.1
Host: localhost:5099
Content-Type: application/json
Accept: application/json

{
  "tipo": "Matriz",
  "cnpj": "12.345.678/0001-90",
  "razaoSocial": "Academia de Kung Fu LTDA",
  "nomeFantasia": "Academia Master",
  "inscricaoEstadual": "123.456.789.012",
  "inscricaoMunicipal": "987654",
  "cnaePrincipal": "8591-1/00",
  "cnaeSecundarios": "8592-9/01, 9312-3/00",
  "regimeTributario": "Simples Nacional",
  "logradouro": "Rua das Artes Marciais",
  "numero": "100",
  "complemento": "Sala 201",
  "bairro": "Centro",
  "cidade": "São Paulo",
  "uf": "SP",
  "cep": "01234-567",
  "pais": "Brasil",
  "telefoneFixo": "(11) 3333-4444",
  "celularWhatsApp": "(11) 98765-4321",
  "email": "contato@academiamaster.com.br",
  "site": "https://www.academiamaster.com.br",
  "nomeResponsavel": "João Silva",
  "idUsuarioCadastrou": "660e8400-e29b-41d4-a716-446655440000"
}
```

### cURL - Criar Matriz
```bash
curl -X POST "http://localhost:5099/api/EscolaKungFu" \
  -H "Content-Type: application/json" \
  -H "accept: application/json" \
  -d '{
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
    "idUsuarioCadastrou": "660e8400-e29b-41d4-a716-446655440000"
  }'
```

### Request - Criar Filial
```http
POST /api/EscolaKungFu HTTP/1.1
Host: localhost:5099
Content-Type: application/json

{
  "tipo": "Filial",
  "cnpj": "12.345.678/0002-71",
  "razaoSocial": "Academia de Kung Fu LTDA",
  "nomeFantasia": "Academia Master - Filial Centro",
  "idEmpresaMatriz": "550e8400-e29b-41d4-a716-446655440000",
  "codigoFilial": "FIL-001",
  "logradouro": "Avenida Paulista",
  "numero": "1500",
  "complemento": "Loja 5",
  "bairro": "Bela Vista",
  "cidade": "São Paulo",
  "uf": "SP",
  "cep": "01310-100",
  "celularWhatsApp": "(11) 97777-8888",
  "email": "filialcentro@academiamaster.com.br",
  "nomeResponsavel": "Maria Santos",
  "idUsuarioCadastrou": "660e8400-e29b-41d4-a716-446655440000"
}
```

### Response 201 Created
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "tipo": "Matriz",
  "eMatriz": true,
  "cnpj": "12.345.678/0001-90",
  "razaoSocial": "Academia de Kung Fu LTDA",
  "nomeFantasia": "Academia Master",
  "logradouro": "Rua das Artes Marciais",
  "numero": "100",
  "bairro": "Centro",
  "cidade": "São Paulo",
  "uf": "SP",
  "cep": "01234-567",
  "dataCriacao": "2025-12-02T14:30:00",
  "ativo": true,
  "cadastroAtivo": true,
  "idUsuarioCadastrou": "660e8400-e29b-41d4-a716-446655440000"
}
```

**Location Header**:
```
Location: /api/EscolaKungFu/550e8400-e29b-41d4-a716-446655440000
```

### Response 400 Bad Request (Validação)
```json
{
  "message": "CNPJ já cadastrado"
}
```

```json
{
  "message": "Filial deve ter uma matriz associada"
}
```

```json
{
  "errors": {
    "CNPJ": ["CNPJ é obrigatório"],
    "RazaoSocial": ["Razão Social é obrigatória"],
    "IdUsuarioCadastrou": ["ID do usuário que cadastrou é obrigatório"]
  }
}
```

---

## 3️⃣ GET /api/EscolaKungFu/{id}
**Descrição**: Obter os dados de uma escola específica pelo ID (GUID).

### Request
```http
GET /api/EscolaKungFu/550e8400-e29b-41d4-a716-446655440000 HTTP/1.1
Host: localhost:5099
Accept: application/json
```

### cURL
```bash
curl -X GET "http://localhost:5099/api/EscolaKungFu/550e8400-e29b-41d4-a716-446655440000" \
  -H "accept: application/json"
```

### Response 200 OK
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "tipo": "Matriz",
  "eMatriz": true,
  "cnpj": "12.345.678/0001-90",
  "razaoSocial": "Academia de Kung Fu LTDA",
  "nomeFantasia": "Academia Master",
  "inscricaoEstadual": "123.456.789.012",
  "inscricaoMunicipal": "987654",
  "cnaePrincipal": "8591-1/00",
  "cnaeSecundarios": "8592-9/01, 9312-3/00",
  "regimeTributario": "Simples Nacional",
  "logradouro": "Rua das Artes Marciais",
  "numero": "100",
  "complemento": "Sala 201",
  "bairro": "Centro",
  "cidade": "São Paulo",
  "uf": "SP",
  "cep": "01234-567",
  "pais": "Brasil",
  "telefoneFixo": "(11) 3333-4444",
  "celularWhatsApp": "(11) 98765-4321",
  "email": "contato@academiamaster.com.br",
  "site": "https://www.academiamaster.com.br",
  "nomeResponsavel": "João Silva",
  "quantidadeFiliais": 3,
  "inscricoesAutorizacoes": "CREF 123456",
  "idEmpresaMatriz": null,
  "codigoFilial": null,
  "dataCriacao": "2025-12-02T10:30:00",
  "dataAlteracao": "2025-12-02T15:45:00",
  "ativo": true,
  "cadastroAtivo": true,
  "idUsuarioCadastrou": "660e8400-e29b-41d4-a716-446655440000",
  "idUsuarioAlterou": "660e8400-e29b-41d4-a716-446655440001"
}
```

### Response 404 Not Found
```json
{
  "message": "Escola não encontrada"
}
```

### Response 400 Bad Request (GUID inválido)
```http
HTTP/1.1 400 Bad Request

The value 'abc123' is not valid for parameter 'id'.
```

---

## 4️⃣ PUT /api/EscolaKungFu/{id}
**Descrição**: Atualizar os dados de uma escola existente.

### Request
```http
PUT /api/EscolaKungFu/550e8400-e29b-41d4-a716-446655440000 HTTP/1.1
Host: localhost:5099
Content-Type: application/json

{
  "razaoSocial": "Academia de Kung Fu LTDA - Atualizada",
  "nomeFantasia": "Academia Master Premium",
  "inscricaoEstadual": "123.456.789.013",
  "inscricaoMunicipal": "987655",
  "cnaePrincipal": "8591-1/00",
  "cnaeSecundarios": "8592-9/01",
  "regimeTributario": "Lucro Presumido",
  "logradouro": "Rua das Artes Marciais",
  "numero": "100-A",
  "complemento": "Andar Térreo",
  "bairro": "Centro",
  "cidade": "São Paulo",
  "uf": "SP",
  "cep": "01234-567",
  "pais": "Brasil",
  "telefoneFixo": "(11) 3333-5555",
  "celularWhatsApp": "(11) 98765-4321",
  "email": "novo@academiamaster.com.br",
  "site": "https://www.academiamaster.com.br",
  "nomeResponsavel": "João Silva Jr.",
  "idUsuarioAlterou": "660e8400-e29b-41d4-a716-446655440001"
}
```

### cURL
```bash
curl -X PUT "http://localhost:5099/api/EscolaKungFu/550e8400-e29b-41d4-a716-446655440000" \
  -H "Content-Type: application/json" \
  -H "accept: application/json" \
  -d '{
    "razaoSocial": "Academia de Kung Fu LTDA - Atualizada",
    "nomeFantasia": "Academia Master Premium",
    "logradouro": "Rua das Artes Marciais",
    "numero": "100-A",
    "bairro": "Centro",
    "cidade": "São Paulo",
    "uf": "SP",
    "cep": "01234-567",
    "pais": "Brasil",
    "email": "novo@academiamaster.com.br",
    "celularWhatsApp": "(11) 98765-4321",
    "idUsuarioAlterou": "660e8400-e29b-41d4-a716-446655440001"
  }'
```

### Response 200 OK
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "tipo": "Matriz",
  "eMatriz": true,
  "cnpj": "12.345.678/0001-90",
  "razaoSocial": "Academia de Kung Fu LTDA - Atualizada",
  "nomeFantasia": "Academia Master Premium",
  "logradouro": "Rua das Artes Marciais",
  "numero": "100-A",
  "complemento": "Andar Térreo",
  "bairro": "Centro",
  "cidade": "São Paulo",
  "uf": "SP",
  "cep": "01234-567",
  "email": "novo@academiamaster.com.br",
  "dataCriacao": "2025-12-02T10:30:00",
  "dataAlteracao": "2025-12-02T16:20:00",
  "ativo": true,
  "cadastroAtivo": true,
  "idUsuarioCadastrou": "660e8400-e29b-41d4-a716-446655440000",
  "idUsuarioAlterou": "660e8400-e29b-41d4-a716-446655440001"
}
```

### Response 404 Not Found
```json
{
  "message": "Escola não encontrada"
}
```

### Response 400 Bad Request
```json
{
  "errors": {
    "RazaoSocial": ["Razão Social é obrigatória"],
    "IdUsuarioAlterou": ["ID do usuário que alterou é obrigatório"]
  }
}
```

---

## 5️⃣ DELETE /api/EscolaKungFu/{id}
**Descrição**: Excluir uma escola do cadastro (soft delete - apenas desativa).

### Request
```http
DELETE /api/EscolaKungFu/550e8400-e29b-41d4-a716-446655440000 HTTP/1.1
Host: localhost:5099
```

### cURL
```bash
curl -X DELETE "http://localhost:5099/api/EscolaKungFu/550e8400-e29b-41d4-a716-446655440000" \
  -H "accept: */*"
```

### Response 204 No Content
```http
HTTP/1.1 204 No Content
```
*(Sem corpo de resposta)*

### Response 404 Not Found
```json
{
  "message": "Escola não encontrada"
}
```

⚠️ **Nota**: Este endpoint faz **soft delete**, ou seja, apenas marca `Ativo = false`. Os dados não são apagados do banco.

---

## 6️⃣ GET /api/EscolaKungFu/matrizes
**Descrição**: Listar apenas as escolas matrizes.

### Request
```http
GET /api/EscolaKungFu/matrizes HTTP/1.1
Host: localhost:5099
Accept: application/json
```

### cURL
```bash
curl -X GET "http://localhost:5099/api/EscolaKungFu/matrizes" \
  -H "accept: application/json"
```

### Response 200 OK
```json
[
  {
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "tipo": "Matriz",
    "eMatriz": true,
    "cnpj": "12.345.678/0001-90",
    "razaoSocial": "Academia de Kung Fu LTDA",
    "nomeFantasia": "Academia Master",
    "logradouro": "Rua das Artes Marciais",
    "numero": "100",
    "bairro": "Centro",
    "cidade": "São Paulo",
    "uf": "SP",
    "cep": "01234-567",
    "quantidadeFiliais": 3,
    "inscricoesAutorizacoes": "CREF 123456",
    "idEmpresaMatriz": null,
    "dataCriacao": "2025-12-02T10:30:00",
    "ativo": true,
    "cadastroAtivo": true
  },
  {
    "id": "550e8400-e29b-41d4-a716-446655440010",
    "tipo": "Matriz",
    "eMatriz": true,
    "cnpj": "98.765.432/0001-10",
    "razaoSocial": "Centro de Artes Marciais XYZ LTDA",
    "nomeFantasia": "Centro XYZ",
    "logradouro": "Av. dos Esportes",
    "numero": "500",
    "bairro": "Jardim América",
    "cidade": "Rio de Janeiro",
    "uf": "RJ",
    "cep": "22011-000",
    "quantidadeFiliais": 5,
    "inscricoesAutorizacoes": null,
    "idEmpresaMatriz": null,
    "dataCriacao": "2025-12-01T09:00:00",
    "ativo": true,
    "cadastroAtivo": true
  }
]
```

### Response 200 OK (Vazio)
```json
[]
```

---

## 7️⃣ GET /api/EscolaKungFu/filiais/{matrizId}
**Descrição**: Listar as filiais de uma escola matriz específica.

### Request
```http
GET /api/EscolaKungFu/filiais/550e8400-e29b-41d4-a716-446655440000 HTTP/1.1
Host: localhost:5099
Accept: application/json
```

### cURL
```bash
curl -X GET "http://localhost:5099/api/EscolaKungFu/filiais/550e8400-e29b-41d4-a716-446655440000" \
  -H "accept: application/json"
```

### Response 200 OK
```json
[
  {
    "id": "550e8400-e29b-41d4-a716-446655440001",
    "tipo": "Filial",
    "eMatriz": false,
    "cnpj": "12.345.678/0002-71",
    "razaoSocial": "Academia de Kung Fu LTDA",
    "nomeFantasia": "Academia Master - Filial Centro",
    "logradouro": "Avenida Paulista",
    "numero": "1500",
    "bairro": "Bela Vista",
    "cidade": "São Paulo",
    "uf": "SP",
    "cep": "01310-100",
    "idEmpresaMatriz": "550e8400-e29b-41d4-a716-446655440000",
    "codigoFilial": "FIL-001",
    "dataCriacao": "2025-12-02T11:00:00",
    "ativo": true
  },
  {
    "id": "550e8400-e29b-41d4-a716-446655440002",
    "tipo": "Filial",
    "eMatriz": false,
    "cnpj": "12.345.678/0003-52",
    "razaoSocial": "Academia de Kung Fu LTDA",
    "nomeFantasia": "Academia Master - Filial Zona Sul",
    "logradouro": "Rua Augusta",
    "numero": "800",
    "bairro": "Consolação",
    "cidade": "São Paulo",
    "uf": "SP",
    "cep": "01305-000",
    "idEmpresaMatriz": "550e8400-e29b-41d4-a716-446655440000",
    "codigoFilial": "FIL-002",
    "dataCriacao": "2025-12-02T12:30:00",
    "ativo": true
  }
]
```

### Response 200 OK (Matriz sem filiais)
```json
[]
```

---

## 8️⃣ GET /api/EscolaKungFu/cnpj/{cnpj}
**Descrição**: Consultar uma escola pelo CNPJ.

### Request
```http
GET /api/EscolaKungFu/cnpj/12.345.678%2F0001-90 HTTP/1.1
Host: localhost:5099
Accept: application/json
```

⚠️ **Nota**: O `/` no CNPJ deve ser codificado como `%2F` na URL.

### cURL
```bash
# Com encoding automático
curl -X GET "http://localhost:5099/api/EscolaKungFu/cnpj/12.345.678/0001-90" \
  -H "accept: application/json"

# Com encoding manual
curl -X GET "http://localhost:5099/api/EscolaKungFu/cnpj/12.345.678%2F0001-90" \
  -H "accept: application/json"
```

### Response 200 OK
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "tipo": "Matriz",
  "eMatriz": true,
  "cnpj": "12.345.678/0001-90",
  "razaoSocial": "Academia de Kung Fu LTDA",
  "nomeFantasia": "Academia Master",
  "inscricaoEstadual": "123.456.789.012",
  "inscricaoMunicipal": "987654",
  "cnaePrincipal": "8591-1/00",
  "logradouro": "Rua das Artes Marciais",
  "numero": "100",
  "bairro": "Centro",
  "cidade": "São Paulo",
  "uf": "SP",
  "cep": "01234-567",
  "telefoneFixo": "(11) 3333-4444",
  "celularWhatsApp": "(11) 98765-4321",
  "email": "contato@academiamaster.com.br",
  "site": "https://www.academiamaster.com.br",
  "nomeResponsavel": "João Silva",
  "quantidadeFiliais": 3,
  "dataCriacao": "2025-12-02T10:30:00",
  "ativo": true,
  "cadastroAtivo": true,
  "idUsuarioCadastrou": "660e8400-e29b-41d4-a716-446655440000"
}
```

### Response 404 Not Found
```json
{
  "message": "Escola não encontrada com o CNPJ informado"
}
```

---

## 🔐 Códigos de Status HTTP

| Código | Descrição | Quando Ocorre |
|--------|-----------|---------------|
| 200 | OK | Requisição bem-sucedida (GET, PUT) |
| 201 | Created | Recurso criado com sucesso (POST) |
| 204 | No Content | Exclusão bem-sucedida (DELETE) |
| 400 | Bad Request | Validação falhou, dados inválidos |
| 404 | Not Found | Recurso não encontrado |
| 500 | Internal Server Error | Erro no servidor |

---

## 🧪 Testando com Postman

### Importar Collection

Crie uma nova Collection no Postman com os seguintes requests:

1. **Listar Todas** - GET `{{baseUrl}}/api/EscolaKungFu`
2. **Criar Matriz** - POST `{{baseUrl}}/api/EscolaKungFu`
3. **Criar Filial** - POST `{{baseUrl}}/api/EscolaKungFu`
4. **Buscar por ID** - GET `{{baseUrl}}/api/EscolaKungFu/{{escolaId}}`
5. **Atualizar** - PUT `{{baseUrl}}/api/EscolaKungFu/{{escolaId}}`
6. **Excluir** - DELETE `{{baseUrl}}/api/EscolaKungFu/{{escolaId}}`
7. **Listar Matrizes** - GET `{{baseUrl}}/api/EscolaKungFu/matrizes`
8. **Listar Filiais** - GET `{{baseUrl}}/api/EscolaKungFu/filiais/{{matrizId}}`
9. **Buscar por CNPJ** - GET `{{baseUrl}}/api/EscolaKungFu/cnpj/{{cnpj}}`

### Variáveis de Ambiente

```json
{
  "baseUrl": "http://localhost:5099",
  "escolaId": "550e8400-e29b-41d4-a716-446655440000",
  "matrizId": "550e8400-e29b-41d4-a716-446655440000",
  "cnpj": "12.345.678/0001-90",
  "usuarioId": "660e8400-e29b-41d4-a716-446655440000"
}
```

---

## 📝 Validações Importantes

### Campos Obrigatórios (POST)
- ✅ `tipo` - "Matriz" ou "Filial"
- ✅ `cnpj` - 14-18 caracteres
- ✅ `razaoSocial` - Máx 200 caracteres
- ✅ `logradouro` - Máx 200 caracteres
- ✅ `numero` - Máx 10 caracteres
- ✅ `bairro` - Máx 100 caracteres
- ✅ `cidade` - Máx 100 caracteres
- ✅ `uf` - Exatamente 2 caracteres
- ✅ `cep` - Máx 10 caracteres
- ✅ `idUsuarioCadastrou` - GUID válido

### Campos Obrigatórios para Filial
- ✅ `idEmpresaMatriz` - GUID da matriz (deve existir)

### Campos Obrigatórios (PUT)
- ✅ `idUsuarioAlterou` - GUID válido
- ✅ Todos os campos de endereço

### Formatos
- **GUID**: `550e8400-e29b-41d4-a716-446655440000`
- **CNPJ**: `12.345.678/0001-90` ou `12345678000190`
- **UF**: `SP`, `RJ`, `MG`, etc. (2 letras)
- **Email**: formato válido (ex: `email@dominio.com`)

---

## 🎯 Fluxo Completo de Teste

### 1. Criar uma Matriz
```bash
POST /api/EscolaKungFu
# Guardar o ID retornado
```

### 2. Criar Filiais para a Matriz
```bash
POST /api/EscolaKungFu
# Usar o ID da matriz em idEmpresaMatriz
```

### 3. Listar Todas as Escolas
```bash
GET /api/EscolaKungFu
# Deve mostrar matriz + filiais
```

### 4. Listar Apenas Matrizes
```bash
GET /api/EscolaKungFu/matrizes
# Deve mostrar apenas a matriz
```

### 5. Listar Filiais da Matriz
```bash
GET /api/EscolaKungFu/filiais/{matrizId}
# Deve mostrar as filiais
```

### 6. Buscar por CNPJ
```bash
GET /api/EscolaKungFu/cnpj/12.345.678/0001-90
# Deve encontrar a escola
```

### 7. Atualizar Dados
```bash
PUT /api/EscolaKungFu/{id}
# Atualizar informações
```

### 8. Excluir (Soft Delete)
```bash
DELETE /api/EscolaKungFu/{id}
# Escola fica inativa
```

---

## 📊 Dados de Teste Adicionais

### Matriz 1 - Academia Premium
```json
{
  "tipo": "Matriz",
  "cnpj": "11.222.333/0001-44",
  "razaoSocial": "Academia Premium de Artes Marciais LTDA",
  "nomeFantasia": "Premium Kung Fu",
  "logradouro": "Rua dos Campeões",
  "numero": "250",
  "bairro": "Jardim Paulista",
  "cidade": "São Paulo",
  "uf": "SP",
  "cep": "01405-000",
  "email": "contato@premiumkungfu.com",
  "celularWhatsApp": "(11) 99999-8888",
  "idUsuarioCadastrou": "660e8400-e29b-41d4-a716-446655440000"
}
```

### Filial 1 - Academia Premium
```json
{
  "tipo": "Filial",
  "cnpj": "11.222.333/0002-25",
  "razaoSocial": "Academia Premium de Artes Marciais LTDA",
  "nomeFantasia": "Premium Kung Fu - Shopping",
  "idEmpresaMatriz": "{{id-da-matriz-premium}}",
  "codigoFilial": "SHOP-01",
  "logradouro": "Av. Brigadeiro Faria Lima",
  "numero": "3000",
  "complemento": "Shopping Piso 3",
  "bairro": "Itaim Bibi",
  "cidade": "São Paulo",
  "uf": "SP",
  "cep": "01452-000",
  "email": "shopping@premiumkungfu.com",
  "celularWhatsApp": "(11) 98888-7777",
  "nomeResponsavel": "Carlos Mendes",
  "idUsuarioCadastrou": "660e8400-e29b-41d4-a716-446655440000"
}
```

---

**Última atualização**: 02/12/2025
**Versão da API**: 1.0.0
