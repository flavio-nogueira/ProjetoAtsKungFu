-- Script para corrigir o hash de senha do usuário flavio.nogueira.alfa@outlook.com.br
-- Copia o hash válido do usuário admin@atskungfu.com

USE dbatskungfu;

UPDATE AspNetUsers u1
SET u1.PasswordHash = (
    SELECT u2.PasswordHash
    FROM AspNetUsers u2
    WHERE u2.Email = 'admin@atskungfu.com'
    LIMIT 1
)
WHERE u1.Email = 'flavio.nogueira.alfa@outlook.com.br';

-- Verificar se a atualização funcionou
SELECT Email, SUBSTRING(PasswordHash, 1, 50) AS PasswordHashPreview
FROM AspNetUsers
WHERE Email IN ('flavio.nogueira.alfa@outlook.com.br', 'admin@atskungfu.com');
