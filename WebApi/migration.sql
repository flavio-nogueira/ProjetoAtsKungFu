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

COMMIT;

