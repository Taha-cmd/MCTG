using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MCTGClassLibrary.DataObjects
{
    [Serializable()]
    public class UserStats : ISerializable
    {
        public int? UserId { get; set; } = null;
        public string Username { get; set; } = null;
        public int? Points { get; set; } = null;
        public int? Battles { get; set; } = null;
        public int? WonBattles { get; set; } = null;
        public int? LostBattles { get; set; } = null;
        public double WinRatio { get; set; } = 0;
        public double LoseRatio { get; set; } = 0;

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Username", Username);
            info.AddValue("UserId", UserId);
            info.AddValue("Points", Points);
            info.AddValue("Battles", Battles);
            info.AddValue("WonBattles", WonBattles);
            info.AddValue("LostBattles", LostBattles);
            info.AddValue("WinRatio", WinRatio);
            info.AddValue("LoseRatio", LoseRatio);
        }
    }
}
