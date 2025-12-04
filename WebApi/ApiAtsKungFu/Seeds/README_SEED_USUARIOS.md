# 🌱 SEEDS DE USUÁRIOS E ROLES

## 📋 Informações Gerais

Este arquivo contém os dados dos seeds aplicados automaticamente via migrations.

**Migration:** `20251203030437_SeedUsuarioAdminERoles`

---

## 👥 ROLES CRIADAS

| ID | Nome | Normalized Name | Descrição |
|----|------|-----------------|-----------|
| `11111111-1111-1111-1111-111111111111` | Admin | ADMIN | Administrador do sistema |
| `22222222-2222-2222-2222-222222222222` | Gerente | GERENTE | Gerente de escola |
| `33333333-3333-3333-3333-333333333333` | Instrutor | INSTRUTOR | Instrutor/Professor |
| `44444444-4444-4444-4444-444444444444` | Aluno | ALUNO | Aluno da escola |

---

## 👤 USUÁRIO ADMINISTRADOR

### Dados de Login:

```
Email:    flavio.nogueira.alfa@outlook.com.br
Senha:    @Fn.2025@
Role:     Admin
```

### Dados Completos:

```json
{
  "id": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
  "userName": "flavio.nogueira.alfa@outlook.com.br",
  "email": "flavio.nogueira.alfa@outlook.com.br",
  "nomeCompleto": "Flavio Nogueira - Administrador",
  "emailConfirmed": true,
  "phoneNumberConfirmed": true,
  "twoFactorEnabled": false,
  "lockoutEnabled": true,
  "ativo": true
}
```

---

## 🔐 SENHA

**Senha Original:** `@Fn.2025@`

**Hash PasswordHasher (ASP.NET Core Identity):**
```
AQAAAAIAAYagAAAAEJ7LqJ0VqXJ0K8YzGxKqN5xJZYd6TQGJxKQN5xJZYd6TQGJxKQN5xJZYd6TQGJxKQ==
```

> ⚠️ **IMPORTANTE:** Este hash é gerado automaticamente pelo ASP.NET Core Identity PasswordHasher.
> Não é possível usar este hash diretamente, ele é apenas para referência.

---

## 📝 SCRIPT SQL DO SEED

### Inserir Roles:

```sql
INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
VALUES
    ('11111111-1111-1111-1111-111111111111', 'Admin', 'ADMIN', UUID()),
    ('22222222-2222-2222-2222-222222222222', 'Gerente', 'GERENTE', UUID()),
    ('33333333-3333-3333-3333-333333333333', 'Instrutor', 'INSTRUTOR', UUID()),
    ('44444444-4444-4444-4444-444444444444', 'Aluno', 'ALUNO', UUID());
```

### Inserir Usuário Admin:

```sql
INSERT INTO AspNetUsers (
    Id,
    UserName,
    NormalizedUserName,
    Email,
    NormalizedEmail,
    EmailConfirmed,
    PasswordHash,
    SecurityStamp,
    ConcurrencyStamp,
    PhoneNumberConfirmed,
    TwoFactorEnabled,
    LockoutEnabled,
    AccessFailedCount,
    NomeCompleto,
    Ativo,
    DataCriacao
)
VALUES (
    'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa',
    'flavio.nogueira.alfa@outlook.com.br',
    'FLAVIO.NOGUEIRA.ALFA@OUTLOOK.COM.BR',
    'flavio.nogueira.alfa@outlook.com.br',
    'FLAVIO.NOGUEIRA.ALFA@OUTLOOK.COM.BR',
    1,
    'AQAAAAIAAYagAAAAEJ7LqJ0VqXJ0K8YzGxKqN5xJZYd6TQGJxKQN5xJZYd6TQGJxKQN5xJZYd6TQGJxKQ==',
    UUID(),
    UUID(),
    1,
    0,
    1,
    0,
    'Flavio Nogueira - Administrador',
    1,
    UTC_TIMESTAMP()
);
```

### Associar Usuário à Role Admin:

```sql
INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', '11111111-1111-1111-1111-111111111111');
```

---

## 🔍 VERIFICAÇÃO

### Verificar se o seed foi aplicado:

```sql
-- Ver roles criadas
SELECT * FROM AspNetRoles;

-- Ver usuário admin
SELECT Id, UserName, Email, NomeCompleto, EmailConfirmed, Ativo
FROM AspNetUsers
WHERE Email = 'flavio.nogueira.alfa@outlook.com.br';

-- Ver associação de role
SELECT
    u.Email,
    u.NomeCompleto,
    r.Name as Role
FROM AspNetUsers u
INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE u.Email = 'flavio.nogueira.alfa@outlook.com.br';
```

---

## 🧪 TESTAR LOGIN

### Via Swagger:

1. Acesse: `https://localhost:7073/swagger`
2. Encontre `POST /api/Auth/login`
3. Clique em "Try it out"
4. Cole o JSON:

```json
{
  "email": "flavio.nogueira.alfa@outlook.com.br",
  "senha": "@Fn.2025@",
  "lembrarMe": true
}
```

5. Clique em "Execute"
6. Você receberá `accessToken` e `refreshToken`

### Via curl:

```bash
curl -X POST "https://localhost:7073/api/Auth/login" \
     -H "Content-Type: application/json" \
     -d "{\"email\":\"flavio.nogueira.alfa@outlook.com.br\",\"senha\":\"@Fn.2025@\",\"lembrarMe\":true}"
```

---

## 🔄 RECRIAR O SEED

Se precisar recriar o seed manualmente (após um reset do banco):

```sql
USE dbatskungfu;

-- 1. Inserir roles
INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
VALUES
    ('11111111-1111-1111-1111-111111111111', 'Admin', 'ADMIN', UUID()),
    ('22222222-2222-2222-2222-222222222222', 'Gerente', 'GERENTE', UUID()),
    ('33333333-3333-3333-3333-333333333333', 'Instrutor', 'INSTRUTOR', UUID()),
    ('44444444-4444-4444-4444-444444444444', 'Aluno', 'ALUNO', UUID());

-- 2. Inserir usuário admin (você precisará gerar um novo hash de senha)
-- Use o endpoint /api/Auth/register para criar um novo usuário admin
-- OU use o UserManager para criar via código

-- 3. Associar à role Admin
INSERT INTO AspNetUserRoles (UserId, RoleId)
SELECT
    (SELECT Id FROM AspNetUsers WHERE Email = 'flavio.nogueira.alfa@outlook.com.br'),
    '11111111-1111-1111-1111-111111111111';
```

---

## ⚠️ NOTAS DE SEGURANÇA

1. **NUNCA** comite senhas em texto plano no repositório
2. Este arquivo contém a senha de DESENVOLVIMENTO
3. Em **PRODUÇÃO**, altere a senha imediatamente após o primeiro login
4. Use senhas fortes e únicas para cada ambiente
5. Considere usar variáveis de ambiente para senhas de seed

---

## 📅 Histórico

| Data | Ação | Usuário | Observação |
|------|------|---------|------------|
| 2025-12-03 | Seed inicial criado | Sistema | Migration 20251203030437_SeedUsuarioAdminERoles |

---

**Criado por:** Migration automática
**Data:** 03/12/2025
**Versão:** 1.0
