using ApiAtsKungFu.Infrastructure.Configuration;
using ApiAtsKungFu.Infrastructure.Data;
using ApiAtsKungFu.Middleware;
using Serilog;
using Prometheus;

// Configurar Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build())
    .CreateLogger();

try
{
    Log.Information("Iniciando API AtsKungFu");

    var builder = WebApplication.CreateBuilder(args);

    // Configurar Serilog
    builder.Host.UseSerilog();

    // Configurar todas as dependências da infraestrutura
    builder.Services.AddInfrastructure(builder.Configuration);

    builder.Services.AddControllers();

    // Configurar Swagger/OpenAPI
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerDocumentation();

    // Configurar Health Checks
    builder.Services.AddHealthChecks()
        .AddMySql(
            builder.Configuration.GetConnectionString("DefaultConnection")!,
            name: "mysql",
            tags: new[] { "db", "mysql" });

    var app = builder.Build();

    // ============================================
    // ORDEM DOS MIDDLEWARES É CRUCIAL!
    // ============================================

    // 1. Exception Handler - DEVE SER O PRIMEIRO para capturar todas as exceções
    app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

    // 2. Request/Response Logging - Logo após exception handler para logar tudo
    app.UseMiddleware<RequestResponseLoggingMiddleware>();

    // 3. Serilog Request Logging (logging adicional do Serilog)
    app.UseSerilogRequestLogging();

    // 4. Prometheus métricas
    app.UseHttpMetrics();

    // 5. Configure the HTTP request pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // 6. HTTPS Redirection
    app.UseHttpsRedirection();

    // 7. Authentication & Authorization
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    // Endpoint de métricas do Prometheus
    app.MapMetrics();

    // Endpoint de Health Check
    app.MapHealthChecks("/health");
    app.MapHealthChecks("/health/ready");
    app.MapHealthChecks("/health/live");

    Log.Information("API AtsKungFu iniciada com sucesso");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "API falhou ao iniciar");
    throw;
}
finally
{
    Log.CloseAndFlush();
}
