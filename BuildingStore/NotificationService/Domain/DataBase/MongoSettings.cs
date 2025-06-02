namespace NotificationService.Domain.DataBase
{
    /// <summary>
    /// Класс настроек БД.
    /// </summary>
    public class MongoSettings
    {
        /// <summary>
        /// Строка подключения.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Наименование БД.
        /// </summary>
        public string DatabaseName { get; set; }
    }
}
