namespace UserService.Infrastructure.RefreshTokenSet
{
    /// <summary>
    /// Refresh токен.
    /// </summary>
    public class RefreshToken
    {
        /// <summary>
        /// Refresh токен.
        /// </summary>
        public string Refresh { get; set; }

        /// <summary>
        /// Дата окончания.
        /// </summary>
        public DateTime Expires { get; set; }
    }
}
