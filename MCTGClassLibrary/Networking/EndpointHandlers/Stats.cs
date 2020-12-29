using MCTGClassLibrary.Database.Repositories;
using MCTGClassLibrary.DataObjects;
using MCTGClassLibrary.Networking.HTTP;
using System.Text.Json;

namespace MCTGClassLibrary.Networking.EndpointHandlers
{
    public class Stats : EndpointHandlerBase
    {
        protected override Response GetHandler(Request request)
        {
            if (request.Authorization.IsNullOrWhiteSpace())
                return ResponseManager.BadRequest("Authoriazion Header required");

            if (!Authorized(request.Authorization))
                return ResponseManager.Unauthorized();

            string username = Session.GetUsername(ExtractAuthorizationToken(request.Authorization));
            if (!new UsersRepository().UserExists(username))
                return ResponseManager.NotFound($"Username {username} does not exist");

            UserStats stats = new ScoresRepository().Stats(username);
            return ResponseManager.OK( JsonSerializer.Serialize(stats) );

        }
    }
}
