using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ResearchAtlas.Extensions;

/// <summary>
/// Provides extension methods for enum values.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Gets the <see cref="DescriptionAttribute.Description"/> value for an enum member.
    /// </summary>
    /// <returns>The description, or the enum member name when no description is defined.</returns>
    public static string GetDescription<TEnum>(this TEnum value)
        where TEnum : struct, Enum
    {
        var field = typeof(TEnum).GetField(value.ToString());
        var description = field?.GetCustomAttribute<DescriptionAttribute>();

        return description?.Description ?? value.ToString();
    }

    /// <summary>
    /// Gets the <see cref="DisplayAttribute.Name"/> value for an enum member.
    /// </summary>
    /// <returns>The display name, or the enum member name when no display name is defined.</returns>
    public static string GetDisplayName<TEnum>(this TEnum value)
        where TEnum : struct, Enum
    {
        var field = typeof(TEnum).GetField(value.ToString());
        var display = field?.GetCustomAttribute<DisplayAttribute>();

        return display?.GetName() ?? value.ToString();
    }
}
