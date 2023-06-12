using System;

namespace _GAME._Scripts.Utilities
{
    public static class RandomExtensions
    {
       public static T NextEnum<T>(this Random random, int offset = 0)
       {
           var values = Enum.GetValues(typeof(T));
           return (T)values.GetValue(random.Next(values.Length - offset));
       }    
    }
}