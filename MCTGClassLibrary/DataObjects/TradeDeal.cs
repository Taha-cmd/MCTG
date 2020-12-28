using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using MCTGClassLibrary.Enums;

namespace MCTGClassLibrary.DataObjects
{
    [Serializable()]
    public class TradeDeal : ISerializable
    {
        public string Id { get; set; }
        public string CardId { get; set; }
        public int OwnerId { get; set; }
        public double MinimumDamage { get; set; }
        public double? MaximumWeakness { get; set; } = 0;
        public string CardType { get; set; } = null;
        public string ElementType { get; set; } = null;
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Id", Id);
            info.AddValue("CardId", CardId);
            info.AddValue("MinimumDamage", MinimumDamage);
            info.AddValue("MaximumWeakness", MaximumWeakness);
            info.AddValue("CardType", CardType);
            info.AddValue("ElementType", ElementType);
        }

        public bool Validate()
        {
            return !Id.IsNull() && !CardId.IsNull();
        }
    }
}
