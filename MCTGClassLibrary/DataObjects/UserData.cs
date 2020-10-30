using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MCTGClassLibrary.DataObjects
{
    [Serializable()]
    public class UserData : ISerializable
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Username", Username);
            info.AddValue("Password", Password);
        }
    }
}
