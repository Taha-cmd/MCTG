using System;
using System.Collections.Generic;
using System.Text;

namespace MCTGClassLibrary.Networking.EndpointHandlers
{
    public class Home : IEndpointHandler
    {
        public Response HandleRequest(Request request)
        {
            Response resp = new Response("200", "OK");

            resp.AddPayload("MONSTER CARD TRADING GAME");

            return resp;
        }
    }
}
