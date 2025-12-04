CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;
ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `EscolaKungFu` (
    `Id` int NOT NULL AUTO_INCREMENT,
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
    `IdEmpresaMatriz` int NULL,
    `CodigoFilial` varchar(50) CHARACTER SET utf8mb4 NULL,
    `DataCriacao` datetime(6) NOT NULL,
    `DataAlteracao` datetime(6) NULL,
    `Ativo` tinyint(1) NOT NULL,
    CONSTRAINT `PK_EscolaKungFu` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_EscolaKungFu_EscolaKungFu_IdEmpresaMatriz` FOREIGN KEY (`IdEmpresaMatriz`) REFERENCES `EscolaKungFu` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE UNIQUE INDEX `IX_EscolaKungFu_CNPJ` ON `EscolaKungFu` (`CNPJ`);

CREATE INDEX `IX_EscolaKungFu_IdEmpresaMatriz` ON `EscolaKungFu` (`IdEmpresaMatriz`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20251202220113_InitialCreate', '9.0.0');

ALTER TABLE `EscolaKungFu` MODIFY COLUMN `IdEmpresaMatriz` char(36) COLLATE ascii_general_ci NULL;

ALTER TABLE `EscolaKungFu` MODIFY COLUMN `Id` char(36) COLLATE ascii_general_ci NOT NULL;

ALTER TABLE `EscolaKungFu` ADD `CadastroAtivo` tinyint(1) NOT NULL DEFAULT TRUE;

ALTER TABLE `EscolaKungFu` ADD `IdUsuarioAlterou` char(36) COLLATE ascii_general_ci NULL;

ALTER TABLE `EscolaKungFu` ADD `IdUsuarioCadastrou` char(36) COLLATE ascii_general_ci NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20251202223045_MigracaoGuidComAuditoria', '9.0.0');

CREATE TABLE `AspNetRoles` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Name` varchar(256) CHARACTER SET utf8mb4 NULL,
    `NormalizedName` varchar(256) CHARACTER SET utf8mb4 NULL,
    `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_AspNetRoles` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

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

CREATE TABLE `AspNetRoleClaims` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `RoleId` char(36) COLLATE ascii_general_ci NOT NULL,
    `ClaimType` longtext CHARACTER SET utf8mb4 NULL,
    `ClaimValue` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_AspNetRoleClaims` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `AspNetUserClaims` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserId` char(36) COLLATE ascii_general_ci NOT NULL,
    `ClaimType` longtext CHARACTER SET utf8mb4 NULL,
    `ClaimValue` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_AspNetUserClaims` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `AspNetUserLogins` (
    `LoginProvider` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `ProviderKey` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `ProviderDisplayName` longtext CHARACTER SET utf8mb4 NULL,
    `UserId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_AspNetUserLogins` PRIMARY KEY (`LoginProvider`, `ProviderKey`),
    CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `AspNetUserRoles` (
    `UserId` char(36) COLLATE ascii_general_ci NOT NULL,
    `RoleId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_AspNetUserRoles` PRIMARY KEY (`UserId`, `RoleId`),
    CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `AspNetUserTokens` (
    `UserId` char(36) COLLATE ascii_general_ci NOT NULL,
    `LoginProvider` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Name` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Value` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_AspNetUserTokens` PRIMARY KEY (`UserId`, `LoginProvider`, `Name`),
    CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

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

CREATE INDEX `IX_AspNetRoleClaims_RoleId` ON `AspNetRoleClaims` (`RoleId`);

CREATE UNIQUE INDEX `RoleNameIndex` ON `AspNetRoles` (`NormalizedName`);

CREATE INDEX `IX_AspNetUserClaims_UserId` ON `AspNetUserClaims` (`UserId`);

CREATE INDEX `IX_AspNetUserLogins_UserId` ON `AspNetUserLogins` (`UserId`);

CREATE INDEX `IX_AspNetUserRoles_RoleId` ON `AspNetUserRoles` (`RoleId`);

CREATE INDEX `EmailIndex` ON `AspNetUsers` (`NormalizedEmail`);

CREATE UNIQUE INDEX `UserNameIndex` ON `AspNetUsers` (`NormalizedUserName`);

CREATE INDEX `IX_RefreshTokens_JwtId` ON `RefreshTokens` (`JwtId`);

CREATE UNIQUE INDEX `IX_RefreshTokens_Token` ON `RefreshTokens` (`Token`);

CREATE INDEX `IX_RefreshTokens_UserId` ON `RefreshTokens` (`UserId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20251203024650_AdicionarIdentityEAuth', '9.0.0');

COMMIT;

