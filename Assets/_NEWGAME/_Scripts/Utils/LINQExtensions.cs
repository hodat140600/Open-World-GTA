using System;
using System.Collections.Generic;
using System.Linq;

namespace _NEWGAME._Scripts.Utils
{
    public static class LINQExtensions
    {
        public static bool None<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            return !source.Any(predicate);
        }
    }
}