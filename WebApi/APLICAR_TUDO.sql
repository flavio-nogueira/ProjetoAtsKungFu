-- ============================================
-- SCRIPT COMPLETO - LIMPAR E APLICAR MIGRATIONS
-- Execute este script no MySQL Workbench
-- ============================================

USE `dbatskungfu`;

-- ============================================
-- PASSO 1: LIMPAR BANCO DE DADOS
-- ============================================

SET FOREIGN_KEY_CHECKS = 0;

-- Remover todas as tabelas existentes
DROP TABLE IF EXISTS `EscolaKungFu`;
DROP TABLE IF EXISTS `RefreshTokens`;
DROP TABLE IF EXISTS `AspNetUserTokens`;
DROP TABLE IF EXISTS `AspNetUserRoles`;
DROP TABLE IF EXISTS `AspNetUserLogins`;
DROP TABLE IF EXISTS `AspNetUserClaims`;
DROP TABLE IF EXISTS `AspNetRoleClaims`;
DROP TABLE IF EXISTS `AspNetRoles`;
DROP TABLE IF EXISTS `AspNetUsers`;

SET FOREIGN_KEY_CHECKS = 1;

-- Limpar histórico de migrations
DELETE FROM `__EFMigrationsHistory`;

SELECT 'Banco limpo com sucesso!' AS Status;

-- ============================================
-- PASSO 2: CRIAR TABELAS DO IDENTITY
-- ============================================

-- Tabela de Roles
CREATE TABLE `AspNetRoles` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Name` varchar(256) CHARACTER SET utf8mb4 NULL,
    `NormalizedName` varchar(256) CHARACTER SET utf8mb4 NULL,
    `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_AspNetRoles` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

-- Tabela de Usuários
CREATE TABLE `AspNetUsers` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `NomeCompleto` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `CPF` varchar(14) CHARACTER SET utf8mb4 NULL,
    `DataNascimento` datetime(6) NULL,
    `FotoPerfil` varchar(500) CHARACTER SET utf8mb4 NULL,
    `Ativo` tinyint(1) NOT NULL,
    `DataCriacao` datetime(6) NOT NULL,
    `DataAlteracao` datetime(6) NULL,
    `DataUltimoLogin` datetime(6) NULL,
    `UserName` varchar(256) CHARACTER SET utf8mb4 NULL,
    `NormalizedUserName` varchar(256) CHARACTER SET utf8mb4 NULL,
    `Email` varchar(256) CHARACTER SET utf8mb4 NULL,
    `NormalizedEmail` varchar(256) CHARACTER SET utf8mb4 NULL,
    `EmailConfirmed` tinyint(1) NOT NULL,
    `PasswordHash` longtext CHARACTER SET utf8mb4 NULL,
    `SecurityStamp` longtext CHARACTER SET utf8mb4 NULL,
    `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 NULL,
    `PhoneNumber` longtext CHARACTER SET utf8mb4 NULL,
    `PhoneNumberConfirmed` tinyint(1) NOT NULL,
    `TwoFactorEnabled` tinyint(1) NOT NULL,
    `LockoutEnd` datetime(6) NULL,
    `LockoutEnabled` tinyint(1) NOT NULL,
    `AccessFailedCount` int NOT NULL,
    CONSTRAINT `PK_AspNetUsers` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

-- Tabela de Claims de Roles
CREATE TABLE `AspNetRoleClaims` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `RoleId` char(36) COLLATE ascii_general_ci NOT NULL,
    `ClaimType` longtext CHARACTER SET utf8mb4 NULL,
    `ClaimValue` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_AspNetRoleClaims` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

-- Tabela de Claims de Usuários
CREATE TABLE `AspNetUserClaims` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserId` char(36) COLLATE ascii_general_ci NOT NULL,
    `ClaimType` longtext CHARACTER SET utf8mb4 NULL,
    `ClaimValue` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_AspNetUserClaims` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

-- Tabela de Logins Externos
CREATE TABLE `AspNetUserLogins` (
    `LoginProvider` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `ProviderKey` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `ProviderDisplayName` longtext CHARACTER SET utf8mb4 NULL,
    `UserId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_AspNetUserLogins` PRIMARY KEY (`LoginProvider`, `ProviderKey`),
    CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

-- Tabela de Relacionamento Usuário-Role
CREATE TABLE `AspNetUserRoles` (
    `UserId` char(36) COLLATE ascii_general_ci NOT NULL,
    `RoleId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_AspNetUserRoles` PRIMARY KEY (`UserId`, `RoleId`),
    CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

-- Tabela de Tokens de Usuário
CREATE TABLE `AspNetUserTokens` (
    `UserId` char(36) COLLATE ascii_general_ci NOT NULL,
    `LoginProvider` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Name` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Value` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_AspNetUserTokens` PRIMARY KEY (`UserId`, `LoginProvider`, `Name`),
    CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

-- ============================================
-- PASSO 3: CRIAR TABELA REFRESH TOKENS
-- ============================================

CREATE TABLE `RefreshTokens` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `UserId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Token` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `JwtId` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `IsUsed` tinyint(1) NOT NULL,
    `IsRevoked` tinyint(1) NOT NULL,
    `DataCriacao` datetime(6) NOT NULL,
    `DataExpiracao` datetime(6) NOT NULL,
    `DataUso` datetime(6) NULL,
    `DataRevogacao` datetime(6) NULL,
    `IpAddress` varchar(50) CHARACTER SET utf8mb4 NULL,
    `UserAgent` varchar(500) CHARACTER SET utf8mb4 NULL,
    `SubstituidoPorToken` char(36) COLLATE ascii_general_ci NULL,
    `MotivoRevogacao` varchar(200) CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_RefreshTokens` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_RefreshTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

-- ============================================
-- PASSO 4: CRIAR TABELA ESCOLA KUNG FU
-- ============================================

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
    `Ativo` tinyint(1) NOT NULL,
    `CadastroAtivo` tinyint(1) NOT NULL DEFAULT TRUE,
    `IdUsuarioCadastrou` char(36) COLLATE ascii_general_ci NOT NULL,
    `IdUsuarioAlterou` char(36) COLLATE ascii_general_ci NULL,
    CONSTRAINT `PK_EscolaKungFu` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_EscolaKungFu_EscolaKungFu_IdEmpresaMatriz` FOREIGN KEY (`IdEmpresaMatriz`) REFERENCES `EscolaKungFu` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

-- ============================================
-- PASSO 5: CRIAR ÍNDICES
-- ============================================

-- Índices AspNetRoles
CREATE UNIQUE INDEX `RoleNameIndex` ON `AspNetRoles` (`NormalizedName`);

-- Índices AspNetUsers
CREATE INDEX `EmailIndex` ON `AspNetUsers` (`NormalizedEmail`);
CREATE UNIQUE INDEX `UserNameIndex` ON `AspNetUsers` (`NormalizedUserName`);

-- Índices AspNetRoleClaims
CREATE INDEX `IX_AspNetRoleClaims_RoleId` ON `AspNetRoleClaims` (`RoleId`);

-- Índices AspNetUserClaims
CREATE INDEX `IX_AspNetUserClaims_UserId` ON `AspNetUserClaims` (`UserId`);

-- Índices AspNetUserLogins
CREATE INDEX `IX_AspNetUserLogins_UserId` ON `AspNetUserLogins` (`UserId`);

-- Índices AspNetUserRoles
CREATE INDEX `IX_AspNetUserRoles_RoleId` ON `AspNetUserRoles` (`RoleId`);

-- Índices RefreshTokens
CREATE UNIQUE INDEX `IX_RefreshTokens_Token` ON `RefreshTokens` (`Token`);
CREATE INDEX `IX_RefreshTokens_UserId` ON `RefreshTokens` (`UserId`);
CREATE INDEX `IX_RefreshTokens_JwtId` ON `RefreshTokens` (`JwtId`);

-- Índices EscolaKungFu
CREATE UNIQUE INDEX `IX_EscolaKungFu_CNPJ` ON `EscolaKungFu` (`CNPJ`);
CREATE INDEX `IX_EscolaKungFu_IdEmpresaMatriz` ON `EscolaKungFu` (`IdEmpresaMatriz`);

-- ============================================
-- PASSO 6: REGISTRAR MIGRATIONS
-- ============================================

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`) VALUES
('20251202220113_InitialCreate', '9.0.0'),
('20251202223045_MigracaoGuidComAuditoria', '9.0.0'),
('20251203024650_AdicionarIdentityEAuth', '9.0.0');

-- ============================================
-- PASSO 7: VERIFICAÇÃO
-- ============================================

SELECT 'Script executado com sucesso!' AS Status;

-- Ver tabelas criadas
SHOW TABLES;

-- Ver migrations aplicadas
SELECT * FROM `__EFMigrationsHistory`;

-- Verificar estrutura da tabela AspNetUsers
DESCRIBE `AspNetUsers`;

-- Verificar estrutura da tabela RefreshTokens
DESCRIBE `RefreshTokens`;

-- Verificar estrutura da tabela EscolaKungFu
DESCRIBE `EscolaKungFu`;
