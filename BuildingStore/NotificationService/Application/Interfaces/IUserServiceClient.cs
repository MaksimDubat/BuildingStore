namespace NotificationService.Application.Interfaces
{
    /// <summary>
    /// Интерфейс для взаимодействия с сервисом пользователей.
    /// </summary>
    public interface IUserServiceClient
    {
        /// <summary>
        /// Получение email адресов пользователей.
        /// </summary>
        /// <param name="cancellationToken"></param>
        Task<List<string>> GetAllUserEmailsAsync(CancellationToken cancellationToken);
    }
}
