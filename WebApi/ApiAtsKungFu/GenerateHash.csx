// Script para gerar hash de senha usando dotnet-script
// Execute: dotnet script GenerateHash.csx

#r "nuget: Microsoft.AspNetCore.Identity.EntityFrameworkCore, 10.0.0"

using Microsoft.AspNetCore.Identity;

public class DummyUser : IdentityUser<Guid> { }

var hasher = new PasswordHasher<DummyUser>();
var user = new DummyUser();
var password = "@Fn.2025@";
var hash = hasher.HashPassword(user, password);

Console.WriteLine($"Hash para senha '{password}':");
Console.WriteLine(hash);
