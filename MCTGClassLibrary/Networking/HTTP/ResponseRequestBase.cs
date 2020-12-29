using System;
using System.Collections.Generic;

namespace MCTGClassLibrary
{
    public class ResponseRequestBase
    {
        protected ResponseRequestBase() { }

        public Dictionary<string, string> Values { get; protected set; }

        public virtual void Display(ConsoleColor color = ConsoleColor.Green)
        {
            foreach (KeyValuePair<string, string> kvp in Values)
                PrintInColor(kvp.Key, kvp.Value, color);

            Console.Write('\n');
        }

        protected void PrintInColor(string key, string value, ConsoleColor color)
        {
            Console.Write(key + ": ");
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
