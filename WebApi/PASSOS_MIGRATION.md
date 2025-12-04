# 🔧 PASSOS PARA APLICAR MIGRATION COM GUID

## ⚠️ IMPORTANTE: Leia Antes de Executar!

Este processo vai **APAGAR TODOS OS DADOS** da tabela `EscolaKungFu`.

---

## 📝 OPÇÃO 1: Script SQL Completo (Recomendado)

### Passo 1: Abra o MySQL Workbench

1. Conecte ao servidor:
   - Host: `168.231.95.240`
   - Port: `3306`
   - Username: `root` (ou usuário com privilégios administrativos)

### Passo 2: Execute os Comandos Abaixo

Copie e cole cada bloco no MySQL Workbench e execute (Ctrl+Enter):

```sql
-- 1. Usar o banco de dados
USE `dbatskungfu`;

-- 2. Remover Foreign Key
ALTER TABLE `EscolaKungFu`
DROP FOREIGN KEY `FK_EscolaKungFu_EscolaKungFu_IdEmpresaMatriz`;

-- 3. Remover a tabela
DROP TABLE `EscolaKungFu`;

-- 4. Limpar histórico de migrations
DELETE FROM `__EFMigrationsHistory`
WHERE `MigrationId` IN ('20251202220113_InitialCreate', '20251202223045_MigracaoGuidComAuditoria');

-- 5. Verificar (deve retornar vazio)
SELECT * FROM `__EFMigrationsHistory`;
```

### Passo 3: Aplicar Migration via EF Core

Agora execute no terminal (Windows):

```bash
cd C:\Desenvolvimento\ProjetoAtsKungFu\WebApi\ApiAtsKungFu
dotnet ef database update
```

Você verá:
```
Applying migration '20251202220113_InitialCreate'.
Applying migration '20251202223045_MigracaoGuidComAuditoria'.
Done.
```

---

## 📝 OPÇÃO 2: Script SQL Manual Completo

Se a OPÇÃO 1 não funcionar, execute este script completo:

```sql
USE `dbatskungfu`;

-- Remover constraints e tabela
ALTER TABLE `EscolaKungFu` DROP FOREIGN KEY IF EXISTS `FK_EscolaKungFu_EscolaKungFu_IdEmpresaMatriz`;
DROP TABLE IF EXISTS `EscolaKungFu`;

-- Limpar migrations
DELETE FROM `__EFMigrationsHistory`;

-- Criar tabela com GUID
CREATE TABLE `EscolaKungFu` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Tipo` varchar(10) CHARACTER SET utf8mb4 NOT NULL,
    `EMatriz` tinyint(1) NOT NULL,
    `CNPJ` varchar(18) CHARACTER SET utf8mb4 NOT NULL,
    `RazaoSocial` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `NomeFantasia` varchar(200) CHARACTER SET utf8mb4 NULL,
    `InscricaoEstadual` varchar(20) CHARACTER SET utf8mb4 NULL,
    `InscricaoMunicipal` varchar(20) CHARACTER SET utf8mb4 NULL,
    `CNAEPrincipal` varchar(10) CHARACTER SET utf8mb4 NULL,
    `CNAESecundarios` varchar(500) CHARACTER SET utf8mb4 NULL,
    `RegimeTributario` varchar(50) CHARACTER SET utf8mb4 NULL,
    `Logradouro` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `Numero` varchar(10) CHARACTER SET utf8mb4 NOT NULL,
    `Complemento` varchar(100) CHARACTER SET utf8mb4 NULL,
    `Bairro` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Cidade` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `UF` varchar(2) CHARACTER SET utf8mb4 NOT NULL,
    `CEP` varchar(10) CHARACTER SET utf8mb4 NOT NULL,
    `Pais` varchar(50) CHARACTER SET utf8mb4 NOT NULL DEFAULT 'Brasil',
    `TelefoneFixo` varchar(20) CHARACTER SET utf8mb4 NULL,
    `CelularWhatsApp` varchar(20) CHARACTER SET utf8mb4 NULL,
    `Email` varchar(100) CHARACTER SET utf8mb4 NULL,
    `Site` varchar(200) CHARACTER SET utf8mb4 NULL,
    `NomeResponsavel` varchar(200) CHARACTER SET utf8mb4 NULL,
    `QuantidadeFiliais` int NULL,
    `InscricoesAutorizacoes` varchar(500) CHARACTER SET utf8mb4 NULL,
    `IdEmpresaMatriz` char(36) COLLATE ascii_general_ci NULL,
    `CodigoFilial` varchar(50) CHARACTER SET utf8mb4 NULL,
    `DataCriacao` datetime(6) NOT NULL,
    `DataAlteracao` datetime(6) NULL,
    `Ativo` tinyint(1) NOT NULL DEFAULT 1,
    `CadastroAtivo` tinyint(1) NOT NULL DEFAULT 1,
    `IdUsuarioCadastrou` char(36) COLLATE ascii_general_ci NOT NULL,
    `IdUsuarioAlterou` char(36) COLLATE ascii_general_ci NULL,
    CONSTRAINT `PK_EscolaKungFu` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_EscolaKungFu_EscolaKungFu_IdEmpresaMatriz`
        FOREIGN KEY (`IdEmpresaMatriz`)
        REFERENCES `EscolaKungFu` (`Id`)
        ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

-- Criar índices
CREATE UNIQUE INDEX `IX_EscolaKungFu_CNPJ` ON `EscolaKungFu` (`CNPJ`);
CREATE INDEX `IX_EscolaKungFu_IdEmpresaMatriz` ON `EscolaKungFu` (`IdEmpresaMatriz`);
CREATE INDEX `IX_EscolaKungFu_Tipo` ON `EscolaKungFu` (`Tipo`);
CREATE INDEX `IX_EscolaKungFu_Ativo` ON `EscolaKungFu` (`Ativo`);

-- Registrar migrations
INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`) VALUES
('20251202220113_InitialCreate', '9.0.0'),
('20251202223045_MigracaoGuidComAuditoria', '9.0.0');

-- Verificar
SELECT 'Migration aplicada com sucesso!' AS Status;
DESCRIBE `EscolaKungFu`;
```

---

## ✅ Verificação

Após executar a migration, verifique:

```sql
-- 1. Ver estrutura da tabela
DESCRIBE `EscolaKungFu`;

-- 2. Verificar se Id é char(36)
SHOW CREATE TABLE `EscolaKungFu`;

-- 3. Ver migrations aplicadas
SELECT * FROM `__EFMigrationsHistory`;

-- Deve mostrar:
-- 20251202220113_InitialCreate
-- 20251202223045_MigracaoGuidComAuditoria
```

---

## 🧪 Testar Inserção

Depois de aplicar a migration, teste inserir um registro:

```sql
INSERT INTO `EscolaKungFu` (
    `Id`, `Tipo`, `EMatriz`, `CNPJ`, `RazaoSocial`,
    `Logradouro`, `Numero`, `Bairro`, `Cidade`, `UF`, `CEP`,
    `DataCriacao`, `Ativo`, `CadastroAtivo`, `IdUsuarioCadastrou`
) VALUES (
    '550e8400-e29b-41d4-a716-446655440000',  -- GUID
    'Matriz',
    1,
    '12.345.678/0001-90',
    'Academia Teste LTDA',
    'Rua Teste',
    '100',
    'Centro',
    'São Paulo',
    'SP',
    '01234-567',
    NOW(),
    1,
    1,
    '550e8400-e29b-41d4-a716-446655440001'  -- GUID do usuário
);

-- Verificar
SELECT * FROM `EscolaKungFu`;
```

Se funcionar, DELETE o registro de teste:
```sql
DELETE FROM `EscolaKungFu` WHERE Id = '550e8400-e29b-41d4-a716-446655440000';
```

---

## 🚀 Iniciar a API

Após aplicar a migration com sucesso:

```bash
cd C:\Desenvolvimento\ProjetoAtsKungFu\WebApi\ApiAtsKungFu
dotnet run
```

---

## ❌ Troubleshooting

### Erro: "Table EscolaKungFu doesn't exist"
Execute a OPÇÃO 2 (script SQL manual completo)

### Erro: "Foreign key constraint fails"
Execute os comandos na ordem:
1. DROP FOREIGN KEY
2. DROP TABLE
3. CREATE TABLE com todos os campos
4. Adicione os índices

### Erro: "Column Id cannot be null"
Certifique-se que o campo `Id` é `char(36)` e NOT NULL

### Erro ao executar dotnet ef
Execute o script SQL manual (OPÇÃO 2)

---

## 📋 Checklist

- [ ] Conectado ao MySQL como root
- [ ] Executado comandos de limpeza (DROP)
- [ ] Tabela recriada com GUID
- [ ] Índices criados
- [ ] Migrations registradas no histórico
- [ ] Teste de inserção funcionou
- [ ] API inicia sem erros
- [ ] Swagger acessível
- [ ] Endpoint GET retorna array vazio []

---

**Próximo Passo**: Após completar este checklist, siga para `README_CONFIGURACAO.md`
