using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MCTGClassLibrary.DataObjects
{
    [Serializable()]
    public class UserData : ISerializable
    {
        public string Username { get; set; } = null;
        public string Password { get; set; } = null;
        public string Name { get; set; } = null;
        public string Bio { get; set; } = null;
        public string Image { get; set; } = null;

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Username", Username);
            info.AddValue("Password", Password);
            info.AddValue("Name", Name);
            info.AddValue("Bio", Bio);
            info.AddValue("Image", Image);
        }
    }
}
