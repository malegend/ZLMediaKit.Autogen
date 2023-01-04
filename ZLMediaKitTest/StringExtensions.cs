using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace ZLMediaKitTest
{
    internal static class StringExtensions
    {

        public static T FromJson<T>(this string value, JsonSerializerOptions settings)
        {
            if (string.IsNullOrEmpty(value)) return default(T);
            if (typeof(T) == typeof(string)) return (dynamic)value;
            return JsonSerializer.Deserialize<T>(value, settings);
        }

        public static T FromJson<T>(this string value)
        {
            return FromJson<T>(value, default);
        }
    }
}
