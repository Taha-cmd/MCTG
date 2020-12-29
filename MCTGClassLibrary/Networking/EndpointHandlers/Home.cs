using MCTGClassLibrary.Database.Repositories;
using MCTGClassLibrary.Networking.HTTP;
using System.IO;

namespace MCTGClassLibrary.Networking.EndpointHandlers
{
    public class Home : EndpointHandlerBase
    {
        protected override Response GetHandler(Request request)
        {
            Response resp = ResponseManager.OK();

            if (File.Exists(Config.HOMEPAGE))
                resp.AddPayload(File.ReadAllText(Config.HOMEPAGE));
            else
                resp.AddPayload("MONSTER CARD TRADING GAME");

            return resp;
        }
    }
}
