using System;

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
