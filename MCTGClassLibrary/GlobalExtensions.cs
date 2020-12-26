using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCTGClassLibrary
{
    public static class GlobalExtensions
    {
        public static bool IsOfType<T>(this object obj)
        {
            // compares the type of the current object
            return obj.GetType().Equals(typeof(T));
        }

        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        public static bool IsEven(this int num)
        {
            return num % 2 == 0;
        }

        public static bool IsNumeric(this object obj)
        {

            return
            (
                obj.GetType().Equals(typeof(short)) ||
                obj.GetType().Equals(typeof(int)) ||
                obj.GetType().Equals(typeof(long)) ||
                obj.GetType().Equals(typeof(ushort)) ||
                obj.GetType().Equals(typeof(uint)) ||
                obj.GetType().Equals(typeof(ulong)) ||
                obj.GetType().Equals(typeof(double)) ||
                obj.GetType().Equals(typeof(decimal)) ||
                obj.GetType().Equals(typeof(float))
            );
        }

        public static bool In<T>(this T obj, params T[] list)
        {
            return new List<T>(list).Contains(obj);
        }
        
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);

            // string.IsNullOrWhiteSpace(str)
            // str.IsNullOrWhiteSpace()
        }
        public static T GetValue<T>(this NpgsqlDataReader reader, string key)
        {
            return reader.GetFieldValue<T>(reader.GetOrdinal(key));
        }

        public static void Log(this Exception ex)
        {
            ConsoleColor currentColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine($"Error from {ex.Source}: {ex.Message}");
            Console.WriteLine("Stack Trace: ");
            Console.WriteLine(ex.StackTrace);

            Console.ForegroundColor = currentColor;
        }
    }
}
