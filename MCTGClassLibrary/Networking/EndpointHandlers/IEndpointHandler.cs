using System;
using System.Collections.Generic;
using System.Text;

namespace MCTGClassLibrary.Networking
{
    public interface IEndpointHandler
    {
        Response HandleRequest(Request request);
    }
}
