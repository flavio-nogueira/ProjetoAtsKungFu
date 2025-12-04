namespace ApiAtsKungFu.Domain.Entities
{
    /// <summary>
    /// Entidade para armazenar tokens de refresh com auditoria completa
    /// </summary>
    public class RefreshToken
    {
        /// <summary>
        /// Identificador único do refresh token
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// ID do usuário proprietário do token
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Token de refresh criptografado
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Token JWT associado (armazenado para rastreabilidade)
        /// </summary>
        public string JwtId { get; set; } = string.Empty;

        /// <summary>
        /// Indica se o token já foi usado
        /// </summary>
        public bool IsUsed { get; set; } = false;

        /// <summary>
        /// Indica se o token foi revogado manualmente
        /// </summary>
        public bool IsRevoked { get; set; } = false;

        /// <summary>
        /// Data de criação do token
        /// </summary>
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Data de expiração do refresh token
        /// </summary>
        public DateTime DataExpiracao { get; set; }

        /// <summary>
        /// Data em que o token foi usado (para refresh)
        /// </summary>
        public DateTime? DataUso { get; set; }

        /// <summary>
        /// Data em que o token foi revogado
        /// </summary>
        public DateTime? DataRevogacao { get; set; }

        /// <summary>
        /// IP do dispositivo que criou o token
        /// </summary>
        public string? IpAddress { get; set; }

        /// <summary>
        /// User Agent do dispositivo
        /// </summary>
        public string? UserAgent { get; set; }

        /// <summary>
        /// Token que substituiu este (em caso de rotation)
        /// </summary>
        public Guid? SubstituidoPorToken { get; set; }

        /// <summary>
        /// Motivo da revogação (se aplicável)
        /// </summary>
        public string? MotivoRevogacao { get; set; }

        /// <summary>
        /// Navegação para o usuário
        /// </summary>
        public virtual ApplicationUser User { get; set; } = null!;

        /// <summary>
        /// Verifica se o token é válido (não usado, não revogado, não expirado)
        /// </summary>
        public bool IsValido => !IsUsed && !IsRevoked && DataExpiracao > DateTime.UtcNow;
    }
}
