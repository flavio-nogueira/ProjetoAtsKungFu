-- Script para limpar banco antes de aplicar migration Identity

USE `dbatskungfu`;

-- Remover foreign key e tabela EscolaKungFu antiga
SET FOREIGN_KEY_CHECKS = 0;
DROP TABLE IF EXISTS `EscolaKungFu`;
DROP TABLE IF EXISTS `RefreshTokens`;
SET FOREIGN_KEY_CHECKS = 1;

-- Limpar histórico de migrations
DELETE FROM `__EFMigrationsHistory`;

-- Verificar (deve retornar vazio)
SELECT 'Migrations limpas' AS Status;
SELECT * FROM `__EFMigrationsHistory`;
