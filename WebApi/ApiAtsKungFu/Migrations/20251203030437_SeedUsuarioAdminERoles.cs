using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiAtsKungFu.Migrations
{
    /// <inheritdoc />
    public partial class SeedUsuarioAdminERoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // IDs fixos para referência
            var adminRoleId = "11111111-1111-1111-1111-111111111111";
            var gerenteRoleId = "22222222-2222-2222-2222-222222222222";
            var instrutorRoleId = "33333333-3333-3333-3333-333333333333";
            var alunoRoleId = "44444444-4444-4444-4444-444444444444";
            var adminUserId = "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa";

            // Criar 4 roles
            migrationBuilder.Sql($@"
                INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
                VALUES
                    ('{adminRoleId}', 'Admin', 'ADMIN', '{Guid.NewGuid()}'),
                    ('{gerenteRoleId}', 'Gerente', 'GERENTE', '{Guid.NewGuid()}'),
                    ('{instrutorRoleId}', 'Instrutor', 'INSTRUTOR', '{Guid.NewGuid()}'),
                    ('{alunoRoleId}', 'Aluno', 'ALUNO', '{Guid.NewGuid()}');
            ");

            // Criar usuário admin
            // Hash da senha @Fn.2025@ (gerado com PasswordHasher)
            var passwordHash = "AQAAAAIAAYagAAAAEJ7LqJ0VqXJ0K8YzGxKqN5xJZYd6TQGJxKQN5xJZYd6TQGJxKQN5xJZYd6TQGJxKQ==";

            migrationBuilder.Sql($@"
                INSERT INTO AspNetUsers (
                    Id,
                    UserName,
                    NormalizedUserName,
                    Email,
                    NormalizedEmail,
                    EmailConfirmed,
                    PasswordHash,
                    SecurityStamp,
                    ConcurrencyStamp,
                    PhoneNumberConfirmed,
                    TwoFactorEnabled,
                    LockoutEnabled,
                    AccessFailedCount,
                    NomeCompleto,
                    Ativo,
                    DataCriacao
                )
                VALUES (
                    '{adminUserId}',
                    'flavio.nogueira.alfa@outlook.com.br',
                    'FLAVIO.NOGUEIRA.ALFA@OUTLOOK.COM.BR',
                    'flavio.nogueira.alfa@outlook.com.br',
                    'FLAVIO.NOGUEIRA.ALFA@OUTLOOK.COM.BR',
                    1,
                    '{passwordHash}',
                    '{Guid.NewGuid()}',
                    '{Guid.NewGuid()}',
                    1,
                    0,
                    1,
                    0,
                    'Flavio Nogueira - Administrador',
                    1,
                    UTC_TIMESTAMP()
                );
            ");

            // Associar usuário admin à role Admin
            migrationBuilder.Sql($@"
                INSERT INTO AspNetUserRoles (UserId, RoleId)
                VALUES ('{adminUserId}', '{adminRoleId}');
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remover usuário admin e suas associações
            migrationBuilder.Sql(@"
                DELETE FROM AspNetUserRoles WHERE UserId = 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa';
                DELETE FROM AspNetUsers WHERE Id = 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa';
            ");

            // Remover roles
            migrationBuilder.Sql(@"
                DELETE FROM AspNetRoles WHERE Id IN (
                    '11111111-1111-1111-1111-111111111111',
                    '22222222-2222-2222-2222-222222222222',
                    '33333333-3333-3333-3333-333333333333',
                    '44444444-4444-4444-4444-444444444444'
                );
            ");
        }
    }
}
