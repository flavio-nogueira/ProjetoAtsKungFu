-- =====================================================
-- Script para Aplicar Migration com GUID
-- =====================================================
-- Execute este script no MySQL Workbench ou via linha de comando
-- =====================================================

USE `dbatskungfu`;

-- PASSO 1: Remover Foreign Key
ALTER TABLE `EscolaKungFu` DROP FOREIGN KEY `FK_EscolaKungFu_EscolaKungFu_IdEmpresaMatriz`;

-- PASSO 2: Dropar a tabela existente (isso vai apagar todos os dados!)
DROP TABLE IF EXISTS `EscolaKungFu`;

-- PASSO 3: Remover migration antiga do histórico
DELETE FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251202220113_InitialCreate';

-- PASSO 4: Criar tabela nova com GUID
CREATE TABLE `EscolaKungFu` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,

    -- Dados Básicos
    `Tipo` varchar(10) CHARACTER SET utf8mb4 NOT NULL COMMENT 'Matriz ou Filial',
    `EMatriz` tinyint(1) NOT NULL COMMENT 'Flag indicando se é matriz',
    `CNPJ` varchar(18) CHARACTER SET utf8mb4 NOT NULL,
    `RazaoSocial` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `NomeFantasia` varchar(200) CHARACTER SET utf8mb4 NULL,
    `InscricaoEstadual` varchar(20) CHARACTER SET utf8mb4 NULL,
    `InscricaoMunicipal` varchar(20) CHARACTER SET utf8mb4 NULL,
    `CNAEPrincipal` varchar(10) CHARACTER SET utf8mb4 NULL,
    `CNAESecundarios` varchar(500) CHARACTER SET utf8mb4 NULL,
    `RegimeTributario` varchar(50) CHARACTER SET utf8mb4 NULL,

    -- Endereço
    `Logradouro` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `Numero` varchar(10) CHARACTER SET utf8mb4 NOT NULL,
    `Complemento` varchar(100) CHARACTER SET utf8mb4 NULL,
    `Bairro` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Cidade` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `UF` varchar(2) CHARACTER SET utf8mb4 NOT NULL,
    `CEP` varchar(10) CHARACTER SET utf8mb4 NOT NULL,
    `Pais` varchar(50) CHARACTER SET utf8mb4 NOT NULL DEFAULT 'Brasil',

    -- Contato
    `TelefoneFixo` varchar(20) CHARACTER SET utf8mb4 NULL,
    `CelularWhatsApp` varchar(20) CHARACTER SET utf8mb4 NULL,
    `Email` varchar(100) CHARACTER SET utf8mb4 NULL,
    `Site` varchar(200) CHARACTER SET utf8mb4 NULL,
    `NomeResponsavel` varchar(200) CHARACTER SET utf8mb4 NULL,

    -- Dados Específicos
    `QuantidadeFiliais` int NULL COMMENT 'Usado apenas para Matriz',
    `InscricoesAutorizacoes` varchar(500) CHARACTER SET utf8mb4 NULL COMMENT 'Usado apenas para Matriz',
    `IdEmpresaMatriz` char(36) COLLATE ascii_general_ci NULL COMMENT 'FK para a Matriz (usado apenas para Filial)',
    `CodigoFilial` varchar(50) CHARACTER SET utf8mb4 NULL COMMENT 'Código da Filial',

    -- Metadados e Auditoria
    `DataCriacao` datetime(6) NOT NULL,
    `DataAlteracao` datetime(6) NULL,
    `Ativo` tinyint(1) NOT NULL DEFAULT 1,
    `CadastroAtivo` tinyint(1) NOT NULL DEFAULT 1,
    `IdUsuarioCadastrou` char(36) COLLATE ascii_general_ci NOT NULL,
    `IdUsuarioAlterou` char(36) COLLATE ascii_general_ci NULL,

    -- Constraints
    CONSTRAINT `PK_EscolaKungFu` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_EscolaKungFu_EscolaKungFu_IdEmpresaMatriz`
        FOREIGN KEY (`IdEmpresaMatriz`)
        REFERENCES `EscolaKungFu` (`Id`)
        ON DELETE RESTRICT
) CHARACTER SET=utf8mb4 COMMENT='Tabela de Escolas de Kung Fu com GUID';

-- Índices
CREATE UNIQUE INDEX `IX_EscolaKungFu_CNPJ` ON `EscolaKungFu` (`CNPJ`);
CREATE INDEX `IX_EscolaKungFu_IdEmpresaMatriz` ON `EscolaKungFu` (`IdEmpresaMatriz`);
CREATE INDEX `IX_EscolaKungFu_Tipo` ON `EscolaKungFu` (`Tipo`);
CREATE INDEX `IX_EscolaKungFu_Ativo` ON `EscolaKungFu` (`Ativo`);

-- Registrar ambas as migrations no histórico
INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES
    ('20251202220113_InitialCreate', '9.0.0'),
    ('20251202223045_MigracaoGuidComAuditoria', '9.0.0');

-- Verificação
SELECT 'Tabela recriada com sucesso com GUID!' AS Status;
DESCRIBE `EscolaKungFu`;

-- =====================================================
-- Fim do Script
-- =====================================================
