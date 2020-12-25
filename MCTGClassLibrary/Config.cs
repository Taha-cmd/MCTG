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

        // database access
        public const string HOST        = "localhost";
        public const string PORT        = "5432";
        public const string DATABASE    = "MCTG";
        public const string USERNAME    = "MCTGAdmin";
        public const string PASSWORD    = "123";

        public const int LISTENINGPORT  = 10001;
        public const string PROTOCOL    = "HTTP/1.1";
        public const string HOMEPAGE    = "../../../../home.txt";


        // game
        public const int COINS          = 20;
        public const int PACKAGECOST    = 5;
        public const int DECKSIZE       = 4;

        public const int POINTS         = 100;
        public const int POINTSPERLOSE  = -3;
        public const int POINTSPERWIN   = 5;
    }
}
