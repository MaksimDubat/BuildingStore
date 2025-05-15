namespace NotificationService.Application.DTOs
{
    public class UserResultDto<T>
    {
        public List<T> Data { get; set; } = [];
    }
}
