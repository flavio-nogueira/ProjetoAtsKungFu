# 🔐 MIGRATION COMPLETA - IDENTITY + AUTENTICAÇÃO

## ⚠️ IMPORTANTE: Este processo vai recriar todo o banco de dados!

---

## 📝 PASSO 1: Limpar Banco de Dados

Execute no MySQL Workbench conectado em `168.231.95.240`:

```sql
USE `dbatskungfu`;

-- Remover foreign key e tabela EscolaKungFu antiga
ALTER TABLE `EscolaKungFu` DROP FOREIGN KEY IF EXISTS `FK_EscolaKungFu_EscolaKungFu_IdEmpresaMatriz`;
DROP TABLE IF EXISTS `EscolaKungFu`;

-- Limpar histórico de migrations
DELETE FROM `__EFMigrationsHistory`;

-- Verificar (deve retornar vazio)
SELECT * FROM `__EFMigrationsHistory`;
```

---

## 📝 PASSO 2: Aplicar Migration via EF Core

Execute no terminal Windows:

```bash
cd C:\Desenvolvimento\ProjetoAtsKungFu\WebApi\ApiAtsKungFu
dotnet ef database update
```

Você verá algo como:

```
Applying migration '20251202220113_InitialCreate'.
Applying migration '20251202223045_MigracaoGuidComAuditoria'.
Applying migration '20251203024650_AdicionarIdentityEAuth'.
Done.
```

---

## ✅ PASSO 3: Verificar Tabelas Criadas

Execute no MySQL Workbench:

```sql
USE `dbatskungfu`;

-- Ver todas as tabelas
SHOW TABLES;

-- Deve mostrar:
-- - EscolaKungFu
-- - RefreshTokens
-- - AspNetUsers
-- - AspNetRoles
-- - AspNetUserRoles
-- - AspNetUserClaims
-- - AspNetUserLogins
-- - AspNetUserTokens
-- - AspNetRoleClaims
-- - __EFMigrationsHistory
```

---

## 🧪 PASSO 4: Verificar Estrutura da Tabela EscolaKungFu

```sql
DESCRIBE `EscolaKungFu`;

-- Verificar se Id é char(36)
SHOW CREATE TABLE `EscolaKungFu`;
```

---

## 👤 PASSO 5: Verificar Usuário Admin Criado

Após a aplicação iniciar pela primeira vez, verifique se o usuário admin foi criado:

```sql
USE `dbatskungfu`;

-- Ver usuários criados
SELECT Id, UserName, Email, NomeCompleto, EmailConfirmed, Ativo
FROM AspNetUsers;

-- Ver roles criadas
SELECT * FROM AspNetRoles;

-- Ver usuários com roles
SELECT u.Email, u.NomeCompleto, r.Name as Role
FROM AspNetUsers u
INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
INNER JOIN AspNetRoles r ON ur.RoleId = r.Id;
```

**Usuário Admin esperado:**
- Email: `flavio.nogueira.alfa@outlook.com.br`
- Senha: `@Fn.2025@`
- Role: `Admin`

---

## 🚀 PASSO 6: Iniciar a API

```bash
cd C:\Desenvolvimento\ProjetoAtsKungFu\WebApi\ApiAtsKungFu
dotnet run
```

Você verá nos logs:

```
[INF] Iniciando seed de roles
[INF] Role Admin criada com sucesso
[INF] Role Gerente criada com sucesso
[INF] Role Instrutor criada com sucesso
[INF] Role Aluno criada com sucesso
[INF] Seed de roles concluído
[INF] Usuário criado com sucesso: {guid} - flavio.nogueira.alfa@outlook.com.br
[INF] Usuário admin adicionado à role Admin com sucesso
[INF] Seeds do banco executados com sucesso
[INF] API AtsKungFu iniciada com sucesso
```

---

## 🔑 PASSO 7: Testar Login do Admin

### Via Swagger:

1. Acesse: `https://localhost:7073/swagger`
2. Encontre o endpoint `POST /api/Auth/login`
3. Clique em "Try it out"
4. Use o JSON:

```json
{
  "email": "flavio.nogueira.alfa@outlook.com.br",
  "senha": "@Fn.2025@",
  "lembrarMe": true
}
```

5. Clique em "Execute"
6. Você receberá:

```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "CfDJ8O+...",
  "tokenType": "Bearer",
  "expiresIn": 3600,
  "expiresAt": "2025-12-03T01:47:00Z",
  "usuario": {
    "id": "...",
    "nomeCompleto": "Flavio Nogueira - Administrador",
    "email": "flavio.nogueira.alfa@outlook.com.br",
    ...
  }
}
```

### Via curl:

```bash
curl -X POST "https://localhost:7073/api/Auth/login" \
     -H "Content-Type: application/json" \
     -d "{\"email\":\"flavio.nogueira.alfa@outlook.com.br\",\"senha\":\"@Fn.2025@\",\"lembrarMe\":true}"
```

---

## 📋 PASSO 8: Testar Outros Endpoints de Auth

### 1. Obter dados do usuário (GET /api/Auth/me)

```bash
curl -X GET "https://localhost:7073/api/Auth/me" \
     -H "Authorization: Bearer {seu_access_token}"
```

### 2. Renovar token (POST /api/Auth/refresh)

```bash
curl -X POST "https://localhost:7073/api/Auth/refresh" \
     -H "Content-Type: application/json" \
     -d "{\"accessToken\":\"{seu_access_token}\",\"refreshToken\":\"{seu_refresh_token}\"}"
```

### 3. Logout / Revogar token (POST /api/Auth/revoke)

```bash
curl -X POST "https://localhost:7073/api/Auth/revoke" \
     -H "Content-Type: application/json" \
     -d "{\"refreshToken\":\"{seu_refresh_token}\",\"motivo\":\"Logout voluntário\"}"
```

---

## ❌ Troubleshooting

### Erro: "Foreign key constraint fails"
**Solução**: Execute o PASSO 1 novamente para limpar completamente o banco

### Erro: "Table already exists"
**Solução**:
```sql
DROP TABLE IF EXISTS `EscolaKungFu`, `RefreshTokens`, `AspNetUsers`, `AspNetRoles`,
                     `AspNetUserRoles`, `AspNetUserClaims`, `AspNetUserLogins`,
                     `AspNetUserTokens`, `AspNetRoleClaim`;
DELETE FROM `__EFMigrationsHistory`;
```

### Erro: "Usuário admin não foi criado"
**Solução**: Verifique os logs da aplicação. O seed roda automaticamente na inicialização.

### Erro ao fazer login: "Email ou senha inválidos"
**Solução**:
- Email: `flavio.nogueira.alfa@outlook.com.br`
- Senha: `@Fn.2025@` (sensível a maiúsculas/minúsculas)

---

## 📊 Estrutura Completa do Banco

Após aplicar a migration, você terá:

### Tabelas de Autenticação (Identity):
- `AspNetUsers` - Usuários do sistema
- `AspNetRoles` - Perfis/Roles (Admin, Gerente, Instrutor, Aluno)
- `AspNetUserRoles` - Relacionamento usuário-role
- `AspNetUserClaims` - Claims personalizadas
- `AspNetUserLogins` - Logins externos (Google, Facebook, etc)
- `AspNetUserTokens` - Tokens de recuperação de senha
- `AspNetRoleClaims` - Claims de roles

### Tabelas de Negócio:
- `EscolaKungFu` - Escolas (matrizes e filiais)
- `RefreshTokens` - Tokens de refresh com auditoria completa

---

## 🔐 Roles Criadas Automaticamente

O seed cria 4 roles:

1. **Admin** - Administrador do sistema (usuário padrão)
2. **Gerente** - Gerente de escola
3. **Instrutor** - Instrutor/Professor
4. **Aluno** - Aluno da escola

---

## 🎯 Próximos Passos

1. ✅ Testar todos os 8 endpoints de autenticação no Swagger
2. ✅ Criar usuários via `/api/Auth/register`
3. ✅ Testar login, refresh e revoke
4. ✅ Integrar com Flutter (veja FLUTTER_INTEGRATION.md)
5. ✅ Começar a usar o sistema!

---

**IMPORTANTE**: Após completar todos os passos, a API estará totalmente funcional com autenticação JWT + RefreshToken!
