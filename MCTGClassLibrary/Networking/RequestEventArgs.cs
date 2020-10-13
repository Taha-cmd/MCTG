using System;
using System.Collections.Generic;
using System.Text;

namespace MCTGClassLibrary
{
    public class RequestEventArgs : EventArgs
    {
        public RequestEventArgs()
        {
            Test = "hello world";
        
        }

        public string Test { get; set; }
    }
}
