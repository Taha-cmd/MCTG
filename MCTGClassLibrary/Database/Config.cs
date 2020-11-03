using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MCTGClassLibrary.Database.Repositories
{
    public class Config
    {
        private Config() { }


        // const is by default static
        public const string HOST        = "localhost";
        public const string PORT        = "5432";
        public const string DATABASE    = "MCTG";
        public const string USERNAME    = "MCTGAdmin";
        public const string PASSWORD    = "123";
    }
}
