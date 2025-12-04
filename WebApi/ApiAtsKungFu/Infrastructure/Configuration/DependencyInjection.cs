using ApiAtsKungFu.Application.Interfaces;
using ApiAtsKungFu.Application.Services;
using ApiAtsKungFu.Domain.Entities;
using ApiAtsKungFu.Domain.Interfaces;
using ApiAtsKungFu.Infrastructure.Data;
using ApiAtsKungFu.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ApiAtsKungFu.Infrastructure.Configuration
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Configura a conexão com o banco de dados MySQL
        /// </summary>
        public static IServiceCollection AddDatabase(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Usar versão específica do MySQL em vez de AutoDetect para evitar timeout no startup
            // EnableRetryOnFailure para lidar com falhas transitórias de conexão
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 43)),
                    mySqlOptions => mySqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null)));

            return services;
        }

        /// <summary>
        /// Configura os repositórios da aplicação
        /// </summary>
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IEscolaKungFuRepository, EscolaKungFuRepository>();

            // Adicione outros repositórios aqui conforme necessário
            // services.AddScoped<IOutroRepository, OutroRepository>();

            return services;
        }

        /// <summary>
        /// Configura os serviços da aplicação
        /// </summary>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IEscolaKungFuService, EscolaKungFuService>();
            services.AddScoped<IAuthService, AuthService>();

            // Adicione outros serviços aqui conforme necessário
            // services.AddScoped<IOutroService, OutroService>();

            return services;
        }

        /// <summary>
        /// Configura o ASP.NET Core Identity
        /// </summary>
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                // Configurações de senha
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 6;

                // Configurações de lockout
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // Configurações de usuário
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            return services;
        }

        /// <summary>
        /// Configura autenticação JWT
        /// </summary>
        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JwtSettings:SecretKey não configurado");
            var issuer = jwtSettings["Issuer"] ?? "ApiAtsKungFu";
            var audience = jwtSettings["Audience"] ?? "ApiAtsKungFu";

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false; // Em produção, mude para true
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddAuthorization();

            return services;
        }

        /// <summary>
        /// Configura o AutoMapper
        /// </summary>
        public static IServiceCollection AddAutoMapperConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }

        /// <summary>
        /// Configura o Swagger com documentação XML
        /// </summary>
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                // Incluir comentários XML
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }
            });

            return services;
        }

        /// <summary>
        /// Configura todas as dependências da aplicação
        /// </summary>
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddDatabase(configuration)
                .AddIdentityConfiguration()
                .AddJwtAuthentication(configuration)
                .AddRepositories()
                .AddApplicationServices()
                .AddAutoMapperConfiguration();

            return services;
        }
    }
}
