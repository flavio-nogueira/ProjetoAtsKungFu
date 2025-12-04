# Setup do Banco de Dados - API AtsKungFu

## 🔴 Problema Identificado

A API não consegue aplicar as migrations automaticamente devido a erro de permissão:
```
Access denied for user 'atskungfu'@'%' to database 'atskungfu'
```

## ✅ Solução: Executar Script SQL Manualmente

### Opção 1: Via MySQL Workbench

1. **Abra o MySQL Workbench**

2. **Conecte ao servidor MySQL**
   - Host: `168.231.95.240`
   - Port: `3306`
   - Username: `root` ou usuário com privilégios administrativos

3. **Execute o script**
   - Abra o arquivo: `setup_database.sql`
   - Clique em "Execute" (ícone de raio)
   - Aguarde a conclusão

### Opção 2: Via Linha de Comando MySQL

```bash
# Conectar ao MySQL como root
mysql -h 168.231.95.240 -u root -p

# Executar o script
source C:\Desenvolvimento\ProjetoAtsKungFu\WebApi\setup_database.sql

# Ou executar diretamente
mysql -h 168.231.95.240 -u root -p < setup_database.sql
```

### Opção 3: Copiar e Colar no Terminal MySQL

1. Conecte ao MySQL:
```bash
mysql -h 168.231.95.240 -u root -p
```

2. Copie e cole o conteúdo do arquivo `setup_database.sql` no terminal

## 📋 O que o Script Faz

1. **Cria o banco de dados** `atskungfu` (se não existir)
2. **Concede permissões** ao usuário `atskungfu`
3. **Cria a tabela** `EscolaKungFu` com todos os campos
4. **Cria índices** para otimização:
   - Índice único em `CNPJ`
   - Índice em `IdEmpresaMatriz` (FK)
   - Índice em `Tipo` (Matriz/Filial)
   - Índice em `Ativo`
5. **Registra a migration** no histórico do EF Core

## 🔍 Verificar se Funcionou

Após executar o script, verifique no MySQL:

```sql
-- Ver todas as tabelas
USE atskungfu;
SHOW TABLES;

-- Ver estrutura da tabela
DESCRIBE EscolaKungFu;

-- Ver se há dados (deve estar vazia inicialmente)
SELECT COUNT(*) FROM EscolaKungFu;

-- Verificar permissões do usuário
SHOW GRANTS FOR 'atskungfu'@'%';
```

## ✅ Testar a API Após Setup

1. **Inicie a API:**
```bash
cd WebApi/ApiAtsKungFu
dotnet run
```

2. **Acesse o Swagger:**
   - https://localhost:7073/swagger
   - http://localhost:5099/swagger

3. **Teste os endpoints:**
   - `GET /api/EscolaKungFu` - Deve retornar array vazio `[]`
   - `POST /api/EscolaKungFu` - Crie uma escola de teste

## 🎯 Exemplo de Dados para Testar

Use este JSON no Swagger para criar uma escola matriz:

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
  "celularWhatsApp": "(11) 98765-4321"
}
```

## 🚨 Troubleshooting

### Erro: "Table already exists"
```sql
-- Verificar e remover tabelas existentes (CUIDADO: Apaga dados!)
DROP TABLE IF EXISTS EscolaKungFu;
DROP TABLE IF EXISTS __EFMigrationsHistory;
```

### Erro de Permissão Persiste
```sql
-- Execute como root
GRANT ALL PRIVILEGES ON atskungfu.* TO 'atskungfu'@'%';
GRANT ALL PRIVILEGES ON atskungfu.* TO 'atskungfu'@'localhost';
FLUSH PRIVILEGES;
```

### Verificar Conexão da API
```bash
# Teste a connection string
cd WebApi/ApiAtsKungFu
dotnet ef dbcontext info
```

## 📁 Arquivos Gerados

- `setup_database.sql` - Script SQL completo para executar manualmente
- `migration.sql` - Script gerado pelo EF Core (mais simples)
- `SETUP_BANCO_DADOS.md` - Este documento

## ✅ Checklist de Setup

- [ ] Script SQL executado sem erros
- [ ] Tabela `EscolaKungFu` criada
- [ ] Índices criados
- [ ] Permissões concedidas ao usuário `atskungfu`
- [ ] API iniciada sem erros
- [ ] Swagger acessível
- [ ] Endpoint GET retorna array vazio
- [ ] Endpoint POST cria registro com sucesso

---

**Após executar o setup, a API estará pronta para uso!** 🚀
