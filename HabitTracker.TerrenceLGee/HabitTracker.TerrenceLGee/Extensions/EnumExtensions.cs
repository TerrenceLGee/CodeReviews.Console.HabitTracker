using System.ComponentModel.DataAnnotations;

namespace HabitTracker.TerrenceLGee.Extensions;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum enumValue)
    {
        var fieldInfo = enumValue
            .GetType()
            .GetField(enumValue.ToString());

        return (fieldInfo?.GetCustomAttributes(typeof(DisplayAttribute), false)
            is DisplayAttribute[] { Length: > 0 } descriptionAttributes
            ? descriptionAttributes[0].Name
            : enumValue.ToString()) ?? string.Empty;
    }
}