using ApiAtsKungFu.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ApiAtsKungFu.Infrastructure.Data
{
    /// <summary>
    /// Classe para seed de dados iniciais do banco
    /// </summary>
    public static class DatabaseSeeder
    {
        /// <summary>
        /// Cria o usuário administrador padrão do sistema
        /// </summary>
        public static async Task SeedAdminUserAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            Log.Information("Iniciando seed de usuário admin");

            try
            {
                // Criar role Admin se não existir
                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    var adminRole = new IdentityRole<Guid>
                    {
                        Id = Guid.NewGuid(),
                        Name = "Admin",
                        NormalizedName = "ADMIN"
                    };

                    var roleResult = await roleManager.CreateAsync(adminRole);
                    if (roleResult.Succeeded)
                    {
                        Log.Information("Role Admin criada com sucesso");
                    }
                    else
                    {
                        Log.Error("Erro ao criar role Admin: {Errors}",
                            string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                    }
                }

                // Verificar se admin já existe
                const string adminEmail = "flavio.nogueira.alfa@outlook.com.br";
                var existingAdmin = await userManager.FindByEmailAsync(adminEmail);

                if (existingAdmin != null)
                {
                    Log.Information("Usuário admin já existe: {Email}", adminEmail);
                    return;
                }

                // Criar usuário admin
                var adminUser = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = adminEmail,
                    Email = adminEmail,
                    NomeCompleto = "Flavio Nogueira - Administrador",
                    EmailConfirmed = true,
                    Ativo = true,
                    DataCriacao = DateTime.UtcNow,
                    PhoneNumberConfirmed = true
                };

                const string adminPassword = "@Fn.2025@";
                var createResult = await userManager.CreateAsync(adminUser, adminPassword);

                if (createResult.Succeeded)
                {
                    Log.Information("Usuário admin criado com sucesso: {UserId} - {Email}",
                        adminUser.Id, adminUser.Email);

                    // Adicionar usuário à role Admin
                    var addToRoleResult = await userManager.AddToRoleAsync(adminUser, "Admin");
                    if (addToRoleResult.Succeeded)
                    {
                        Log.Information("Usuário admin adicionado à role Admin com sucesso");
                    }
                    else
                    {
                        Log.Error("Erro ao adicionar usuário admin à role: {Errors}",
                            string.Join(", ", addToRoleResult.Errors.Select(e => e.Description)));
                    }
                }
                else
                {
                    Log.Error("Erro ao criar usuário admin: {Errors}",
                        string.Join(", ", createResult.Errors.Select(e => e.Description)));
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro ao executar seed de usuário admin");
            }
        }

        /// <summary>
        /// Cria roles básicas do sistema
        /// </summary>
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            Log.Information("Iniciando seed de roles");

            string[] roles = { "Admin", "Gerente", "Instrutor", "Aluno" };

            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var role = new IdentityRole<Guid>
                    {
                        Id = Guid.NewGuid(),
                        Name = roleName,
                        NormalizedName = roleName.ToUpper()
                    };

                    var result = await roleManager.CreateAsync(role);
                    if (result.Succeeded)
                    {
                        Log.Information("Role {RoleName} criada com sucesso", roleName);
                    }
                    else
                    {
                        Log.Error("Erro ao criar role {RoleName}: {Errors}", roleName,
                            string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }
            }

            Log.Information("Seed de roles concluído");
        }

        /// <summary>
        /// Executa todos os seeds do sistema
        /// </summary>
        public static async Task ExecutarSeedsAsync(IServiceProvider serviceProvider)
        {
            await SeedRolesAsync(serviceProvider);
            await SeedAdminUserAsync(serviceProvider);
        }
    }
}
