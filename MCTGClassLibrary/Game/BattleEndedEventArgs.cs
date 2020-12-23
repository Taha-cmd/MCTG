using System;
using System.Collections.Generic;
using System.Text;

namespace MCTGClassLibrary.Game
{
    public class BattleEndedEventArgs : EventArgs
    {
        public BattleEndedEventArgs(string log)
        {
            BattleLog = log;
        }

        public string BattleLog { get; private set; }
    }
}
