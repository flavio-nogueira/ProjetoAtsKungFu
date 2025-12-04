using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiAtsKungFu.Migrations
{
    /// <inheritdoc />
    public partial class MigracaoGuidComAuditoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Dropar a foreign key constraint
            migrationBuilder.DropForeignKey(
                name: "FK_EscolaKungFu_EscolaKungFu_IdEmpresaMatriz",
                table: "EscolaKungFu");

            // 2. Alterar o tipo da coluna IdEmpresaMatriz
            migrationBuilder.AlterColumn<Guid>(
                name: "IdEmpresaMatriz",
                table: "EscolaKungFu",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            // 3. Alterar o tipo da coluna Id
            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "EscolaKungFu",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            // 4. Adicionar novas colunas de auditoria
            migrationBuilder.AddColumn<bool>(
                name: "CadastroAtivo",
                table: "EscolaKungFu",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IdUsuarioAlterou",
                table: "EscolaKungFu",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "IdUsuarioCadastrou",
                table: "EscolaKungFu",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            // 5. Recriar a foreign key constraint
            migrationBuilder.AddForeignKey(
                name: "FK_EscolaKungFu_EscolaKungFu_IdEmpresaMatriz",
                table: "EscolaKungFu",
                column: "IdEmpresaMatriz",
                principalTable: "EscolaKungFu",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 1. Dropar foreign key constraint
            migrationBuilder.DropForeignKey(
                name: "FK_EscolaKungFu_EscolaKungFu_IdEmpresaMatriz",
                table: "EscolaKungFu");

            // 2. Remover colunas de auditoria
            migrationBuilder.DropColumn(
                name: "CadastroAtivo",
                table: "EscolaKungFu");

            migrationBuilder.DropColumn(
                name: "IdUsuarioAlterou",
                table: "EscolaKungFu");

            migrationBuilder.DropColumn(
                name: "IdUsuarioCadastrou",
                table: "EscolaKungFu");

            // 3. Reverter tipo da coluna IdEmpresaMatriz para int
            migrationBuilder.AlterColumn<int>(
                name: "IdEmpresaMatriz",
                table: "EscolaKungFu",
                type: "int",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            // 4. Reverter tipo da coluna Id para int
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "EscolaKungFu",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            // 5. Recriar a foreign key constraint
            migrationBuilder.AddForeignKey(
                name: "FK_EscolaKungFu_EscolaKungFu_IdEmpresaMatriz",
                table: "EscolaKungFu",
                column: "IdEmpresaMatriz",
                principalTable: "EscolaKungFu",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
