-- =====================================================
-- Script para Recriar Banco de Dados com GUID
-- =====================================================
-- Execute este script no MySQL para recriar a tabela com GUIDs
-- =====================================================

USE `dbatskungfu`;

-- Remover foreign key constraints primeiro
ALTER TABLE `EscolaKungFu` DROP FOREIGN KEY IF EXISTS `FK_EscolaKungFu_EscolaKungFu_IdEmpresaMatriz`;

-- Dropar tabela existente
DROP TABLE IF EXISTS `EscolaKungFu`;

-- Dropar histórico de migrations
DELETE FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251202220113_InitialCreate';

-- Agora as migrations do EF Core podem ser executadas
-- Execute no terminal: dotnet ef database update

SELECT 'Tabelas removidas com sucesso! Execute: dotnet ef database update' AS Status;
