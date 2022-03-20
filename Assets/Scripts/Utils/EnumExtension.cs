using System;

namespace Utils
{
    public class EnumExtension
    {
        public static T GetEnumByName<T>(string localEnum) where  T : Enum
        {
            return (T)Enum.Parse(typeof(T),localEnum);
        }
    }
}