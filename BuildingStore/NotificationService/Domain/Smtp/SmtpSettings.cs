namespace NotificationService.Domain.Smtp
{
    /// <summary>
    /// Класс настроек Smtp.
    /// </summary>
    public class SmtpSettings
    {
        /// <summary>
        /// Email отправителя.
        /// </summary>
        public string FromEmail { get; set; }

        /// <summary>
        /// Имя отправителя.
        /// </summary>
        public string FromName { get; set; }

        /// <summary>
        /// Адрес SMTP-сервера.
        /// </summary>
        public string SmtpServer { get; set; }

        /// <summary>
        /// Порт SMTP
        /// </summary>
        public int SmtpPort { get; set; }      

        /// <summary>
        /// Использование SSl.
        /// </summary>
        public bool UseSsl { get; set; }

        /// <summary>
        /// Логин SMTP.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Пароль SMTP.
        /// </summary>
        public string Password { get; set; }  
    }
}
