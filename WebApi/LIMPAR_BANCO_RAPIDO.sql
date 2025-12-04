-- Execute este script no MySQL Workbench ANTES de rodar dotnet ef database update

USE `dbatskungfu`;

SET FOREIGN_KEY_CHECKS = 0;
DROP TABLE IF EXISTS `EscolaKungFu`;
DROP TABLE IF EXISTS `RefreshTokens`;
SET FOREIGN_KEY_CHECKS = 1;

DELETE FROM `__EFMigrationsHistory`;

SELECT 'Banco limpo! Agora rode: dotnet ef database update' AS Instrucao;
