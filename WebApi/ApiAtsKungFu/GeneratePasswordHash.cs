using Microsoft.AspNetCore.Identity;
using ApiAtsKungFu.Domain.Entities;

namespace ApiAtsKungFu;

/// <summary>
/// Classe utilitária para gerar hash de senha
/// </summary>
public static class GeneratePasswordHash
{
    public static string Generate(string password)
    {
        var hasher = new PasswordHasher<ApplicationUser>();
        var user = new ApplicationUser();
        return hasher.HashPassword(user, password);
    }
}
