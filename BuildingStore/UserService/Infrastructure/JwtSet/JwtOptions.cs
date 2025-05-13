namespace UserService.Infrastructure.JwtSet
{
    /// <summary>
    /// Класс для хранения настроек Jwt.
    /// </summary>
    public class JwtOptions
    {
        /// <summary>
        /// Издатель токена.
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// Аудитория токена.
        /// </summary>
        public string Audience { get; set; }
        /// <summary>
        /// Ключ.
        /// </summary>
        public string Key { get; set; }
    }
}
