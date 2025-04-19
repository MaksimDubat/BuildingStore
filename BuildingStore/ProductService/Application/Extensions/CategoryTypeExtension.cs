using System.ComponentModel;
using System.Reflection;

namespace ProductService.Application.Extensions
{
    /// <summary>
    /// Расширение для получения описания из перечисления.
    /// </summary>
    public static class CategoryTypeExtension
    {
        public static string GetDescription(this Enum value)
        {
            return value.GetType()
                .GetMember(value.ToString())
                .First()
                .GetCustomAttribute<DescriptionAttribute>()?
                .Description ?? value.ToString();
        }
    }
}
