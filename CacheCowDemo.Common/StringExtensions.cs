using System;

namespace CacheCowDemo.Common
{
    public static class StringExtensions
    {
        public static int GetNthIndexOf(this string src, char value, int n)
        {
            return GetNthIndex(src, value, 0, n); 
        }

        private static int GetNthIndex(string src, char value, int startIndex, int n)
        {
            if (n < 1)
            {
                throw new ArgumentException("Must be greater than 1", "n");
            }

            if (n == 1)
            {
                return src.IndexOf(value, startIndex);
            }

            var nextStartIndex = src.IndexOf(value, startIndex) + 1;
            return GetNthIndex(src, value, nextStartIndex, --n);
        }
    }
}
