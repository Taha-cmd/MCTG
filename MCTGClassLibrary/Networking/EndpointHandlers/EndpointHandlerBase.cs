using MCTGClassLibrary.DataObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCTGClassLibrary.Networking.EndpointHandlers
{
    public class EndpointHandlerBase
    {
        protected EndpointHandlerBase() { }

        protected UserData GetUser(string json)
        {
            throw new NotImplementedException();
        }
    }
}
