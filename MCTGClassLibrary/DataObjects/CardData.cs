using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MCTGClassLibrary.DataObjects
{
    [Serializable()]
    public class CardData : ISerializable
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Damage { get; set; }
        public double Weakness { get; set; } = 0;         //default value

        public CardData(string id, string name, double damage, double weakness)
        {
            Id = id;
            Name = name;
            Damage = damage;
            Weakness = weakness;
        }

        public CardData() { }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Id", Id);
            info.AddValue("Name", Name);
            info.AddValue("Damage", Damage);
            info.AddValue("Weakness", Weakness);
        }
    }
}
