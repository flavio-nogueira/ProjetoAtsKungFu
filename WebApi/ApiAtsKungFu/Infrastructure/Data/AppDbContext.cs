using ApiAtsKungFu.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiAtsKungFu.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<EscolaKungFu> EscolasKungFu { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EscolaKungFu>(entity =>
            {
                entity.ToTable("EscolaKungFu");

                entity.HasKey(e => e.Id);

                // Configurar Id como Guid
                entity.Property(e => e.Id)
                    .HasColumnType("char(36)")
                    .IsRequired();

                // Configurar propriedades
                entity.Property(e => e.Tipo).IsRequired().HasMaxLength(10);
                entity.Property(e => e.CNPJ).IsRequired().HasMaxLength(18);
                entity.Property(e => e.RazaoSocial).IsRequired().HasMaxLength(200);
                entity.Property(e => e.NomeFantasia).HasMaxLength(200);
                entity.Property(e => e.InscricaoEstadual).HasMaxLength(20);
                entity.Property(e => e.InscricaoMunicipal).HasMaxLength(20);
                entity.Property(e => e.CNAEPrincipal).HasMaxLength(10);
                entity.Property(e => e.CNAESecundarios).HasMaxLength(500);
                entity.Property(e => e.RegimeTributario).HasMaxLength(50);

                entity.Property(e => e.Logradouro).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Numero).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Complemento).HasMaxLength(100);
                entity.Property(e => e.Bairro).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Cidade).IsRequired().HasMaxLength(100);
                entity.Property(e => e.UF).IsRequired().HasMaxLength(2);
                entity.Property(e => e.CEP).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Pais).HasMaxLength(50).HasDefaultValue("Brasil");

                entity.Property(e => e.TelefoneFixo).HasMaxLength(20);
                entity.Property(e => e.CelularWhatsApp).HasMaxLength(20);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Site).HasMaxLength(200);
                entity.Property(e => e.NomeResponsavel).HasMaxLength(200);

                entity.Property(e => e.InscricoesAutorizacoes).HasMaxLength(500);
                entity.Property(e => e.CodigoFilial).HasMaxLength(50);

                // Configurar campos de auditoria
                entity.Property(e => e.IdEmpresaMatriz).HasColumnType("char(36)");
                entity.Property(e => e.IdUsuarioCadastrou).HasColumnType("char(36)").IsRequired();
                entity.Property(e => e.IdUsuarioAlterou).HasColumnType("char(36)");
                entity.Property(e => e.CadastroAtivo).IsRequired().HasDefaultValue(true);

                // Índice único para CNPJ
                entity.HasIndex(e => e.CNPJ).IsUnique();

                // Configurar relacionamento de auto-referência
                entity.HasOne(e => e.Matriz)
                    .WithMany()
                    .HasForeignKey(e => e.IdEmpresaMatriz)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configurar RefreshToken
            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("RefreshTokens");

                entity.HasKey(rt => rt.Id);

                entity.Property(rt => rt.Id)
                    .HasColumnType("char(36)")
                    .IsRequired();

                entity.Property(rt => rt.UserId)
                    .HasColumnType("char(36)")
                    .IsRequired();

                entity.Property(rt => rt.Token)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(rt => rt.JwtId)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(rt => rt.IpAddress)
                    .HasMaxLength(50);

                entity.Property(rt => rt.UserAgent)
                    .HasMaxLength(500);

                entity.Property(rt => rt.SubstituidoPorToken)
                    .HasColumnType("char(36)");

                entity.Property(rt => rt.MotivoRevogacao)
                    .HasMaxLength(200);

                // Índices
                entity.HasIndex(rt => rt.Token).IsUnique();
                entity.HasIndex(rt => rt.UserId);
                entity.HasIndex(rt => rt.JwtId);

                // Relacionamento com ApplicationUser
                entity.HasOne(rt => rt.User)
                    .WithMany(u => u.RefreshTokens)
                    .HasForeignKey(rt => rt.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configurar tabelas do Identity com Guid
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(u => u.Id).HasColumnType("char(36)");
                entity.Property(u => u.NomeCompleto).IsRequired().HasMaxLength(200);
                entity.Property(u => u.CPF).HasMaxLength(14);
                entity.Property(u => u.FotoPerfil).HasMaxLength(500);
            });

            modelBuilder.Entity<IdentityRole<Guid>>(entity =>
            {
                entity.Property(r => r.Id).HasColumnType("char(36)");
            });

            modelBuilder.Entity<IdentityUserClaim<Guid>>(entity =>
            {
                entity.Property(uc => uc.UserId).HasColumnType("char(36)");
            });

            modelBuilder.Entity<IdentityUserLogin<Guid>>(entity =>
            {
                entity.Property(ul => ul.UserId).HasColumnType("char(36)");
            });

            modelBuilder.Entity<IdentityUserToken<Guid>>(entity =>
            {
                entity.Property(ut => ut.UserId).HasColumnType("char(36)");
            });

            modelBuilder.Entity<IdentityRoleClaim<Guid>>(entity =>
            {
                entity.Property(rc => rc.RoleId).HasColumnType("char(36)");
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>(entity =>
            {
                entity.Property(ur => ur.UserId).HasColumnType("char(36)");
                entity.Property(ur => ur.RoleId).HasColumnType("char(36)");
            });
        }
    }
}
