using Microsoft.AspNetCore.Identity;
using ApiAtsKungFu.Domain.Entities;

// Gerar hash da senha @Fn.2025@
var hasher = new PasswordHasher<ApplicationUser>();
var user = new ApplicationUser();
var hash = hasher.HashPassword(user, "@Fn.2025@");

Console.WriteLine("Hash da senha @Fn.2025@:");
Console.WriteLine(hash);
