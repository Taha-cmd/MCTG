using MCTGClassLibrary.Database.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MCTGClassLibrary.Networking.EndpointHandlers
{
    public class Home : EndpointHandlerBase
    {
        protected override Response GetHandler(Request request)
        {
            Response resp = new Response("200", "OK");

            if (File.Exists(Config.HOMEPAGE))
                resp.AddPayload(File.ReadAllText(Config.HOMEPAGE));
            else
                resp.AddPayload("MONSTER CARD TRADING GAME");

            return resp;
        }
    }
}
