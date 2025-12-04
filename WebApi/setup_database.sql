-- =====================================================
-- Script para Configuração do Banco de Dados AtsKungFu
-- =====================================================
-- Execute este script no MySQL para criar o banco de dados e as tabelas
-- =====================================================

-- PASSO 1: Garantir que o banco de dados existe
CREATE DATABASE IF NOT EXISTS `atskungfu` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- PASSO 2: Conceder permissões ao usuário
-- Execute como usuário root ou com privilégios administrativos
GRANT ALL PRIVILEGES ON `atskungfu`.* TO 'atskungfu'@'%';
FLUSH PRIVILEGES;

-- PASSO 3: Usar o banco de dados
USE `atskungfu`;

-- =====================================================
-- Criação das Tabelas (Migration)
-- =====================================================

-- Tabela de controle de migrations do Entity Framework
CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

-- Tabela principal: EscolaKungFu
CREATE TABLE IF NOT EXISTS `EscolaKungFu` (
    `Id` int NOT NULL AUTO_INCREMENT,

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
    `IdEmpresaMatriz` int NULL COMMENT 'FK para a Matriz (usado apenas para Filial)',
    `CodigoFilial` varchar(50) CHARACTER SET utf8mb4 NULL COMMENT 'Código da Filial',

    -- Metadados
    `DataCriacao` datetime(6) NOT NULL,
    `DataAlteracao` datetime(6) NULL,
    `Ativo` tinyint(1) NOT NULL DEFAULT 1,

    -- Constraints
    CONSTRAINT `PK_EscolaKungFu` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_EscolaKungFu_EscolaKungFu_IdEmpresaMatriz`
        FOREIGN KEY (`IdEmpresaMatriz`)
        REFERENCES `EscolaKungFu` (`Id`)
        ON DELETE RESTRICT
) CHARACTER SET=utf8mb4 COMMENT='Tabela de Escolas de Kung Fu (Matrizes e Filiais)';

-- Índices
CREATE UNIQUE INDEX `IX_EscolaKungFu_CNPJ` ON `EscolaKungFu` (`CNPJ`);
CREATE INDEX `IX_EscolaKungFu_IdEmpresaMatriz` ON `EscolaKungFu` (`IdEmpresaMatriz`);
CREATE INDEX `IX_EscolaKungFu_Tipo` ON `EscolaKungFu` (`Tipo`);
CREATE INDEX `IX_EscolaKungFu_Ativo` ON `EscolaKungFu` (`Ativo`);

-- Registrar migration no histórico
INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20251202220113_InitialCreate', '9.0.0');

-- =====================================================
-- Verificação (Opcional)
-- =====================================================
-- Verifique se as tabelas foram criadas
SHOW TABLES;

-- Verifique a estrutura da tabela EscolaKungFu
DESCRIBE `EscolaKungFu`;

-- =====================================================
-- Dados de Teste (Opcional)
-- =====================================================
-- Descomente as linhas abaixo para inserir dados de teste

/*
-- Inserir uma Matriz de exemplo
INSERT INTO `EscolaKungFu` (
    `Tipo`, `EMatriz`, `CNPJ`, `RazaoSocial`, `NomeFantasia`,
    `Logradouro`, `Numero`, `Bairro`, `Cidade`, `UF`, `CEP`,
    `DataCriacao`, `Ativo`
) VALUES (
    'Matriz', 1, '12.345.678/0001-90', 'Academia de Kung Fu LTDA', 'Academia Master',
    'Rua das Artes Marciais', '100', 'Centro', 'São Paulo', 'SP', '01234-567',
    NOW(), 1
);

-- Inserir uma Filial de exemplo
INSERT INTO `EscolaKungFu` (
    `Tipo`, `EMatriz`, `CNPJ`, `RazaoSocial`, `NomeFantasia`,
    `IdEmpresaMatriz`,
    `Logradouro`, `Numero`, `Bairro`, `Cidade`, `UF`, `CEP`,
    `DataCriacao`, `Ativo`
) VALUES (
    'Filial', 0, '12.345.678/0002-71', 'Academia de Kung Fu LTDA', 'Academia Filial Centro',
    LAST_INSERT_ID(),
    'Avenida Principal', '200', 'Jardins', 'São Paulo', 'SP', '01234-567',
    NOW(), 1
);

-- Verificar dados inseridos
SELECT * FROM `EscolaKungFu`;
*/

-- =====================================================
-- Fim do Script
-- =====================================================
