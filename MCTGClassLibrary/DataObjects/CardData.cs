using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MCTGClassLibrary.DataObjects
{
    [Serializable()]
    public class CardData : ISerializable
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Damage { get; set; }
        public string Weakness { get; set; }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ID", ID);
            info.AddValue("Name", Name);
            info.AddValue("Damage", Damage);
            info.AddValue("Weakness", Weakness);
        }
    }
}
