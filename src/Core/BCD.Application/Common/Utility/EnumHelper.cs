using BCD.Application.DTOs.Common;
using System.ComponentModel;
using System.Reflection;

namespace BCD.Application.Common.Utility;

public class EnumHelper
{

    public static string GetEnumDescription(Enum value)
    {
        var fi = value.GetType().GetField(value.ToString());
        var attribute = fi.GetCustomAttribute<DescriptionAttribute>();
        return attribute?.Description ?? value.ToString();
    }


    public static List<DropDownDto> GetDropDownList<TEnum>() where TEnum : Enum
    {
        return ((TEnum[])Enum.GetValues(typeof(TEnum)))
                .Select(enumValue => new DropDownDto
                {
                    Id = Convert.ToInt32(enumValue),
                    Name = GetEnumDescriptionByEnum(enumValue)
                })
                .ToList();
    }


    private static string GetEnumDescriptionByEnum(Enum value)
    {
        var fi = value.GetType().GetField(value.ToString());
        var attribute = fi.GetCustomAttribute<DescriptionAttribute>();
        return attribute?.Description ?? value.ToString();
    }

    public static string GetEnumDescriptionUsingEnumFile<TEnum>(int intValue) where TEnum : Enum
    {
        var enumValue = (TEnum)Enum.ToObject(typeof(TEnum), intValue);
        var fi = enumValue.GetType().GetField(enumValue.ToString());

        if (fi != null)
        {
            var attribute = fi.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? enumValue.ToString();
        }

        return enumValue.ToString();
    }

}
