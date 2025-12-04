using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiAtsKungFu.Migrations
{
    /// <inheritdoc />
    public partial class FixPasswordHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Atualizar o hash de senha do usuário flavio.nogueira.alfa@outlook.com.br
            // Copiando o hash válido do usuário admin@atskungfu.com (ambos têm a mesma senha: @Fn.2025@)
            migrationBuilder.Sql(@"
                UPDATE AspNetUsers u1
                SET u1.PasswordHash = (
                    SELECT u2.PasswordHash
                    FROM AspNetUsers u2
                    WHERE u2.Email = 'admin@atskungfu.com'
                    LIMIT 1
                )
                WHERE u1.Email = 'flavio.nogueira.alfa@outlook.com.br';
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
