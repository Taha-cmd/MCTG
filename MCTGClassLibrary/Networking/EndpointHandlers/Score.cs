using MCTGClassLibrary.Database.Repositories;
using MCTGClassLibrary.Networking.HTTP;
using System.Text.Json;

namespace MCTGClassLibrary.Networking.EndpointHandlers
{
    public class Score : EndpointHandlerBase
    {
        // get /score => display all scores sorted
        protected override Response GetHandler(Request request)
        {
            if (request.Authorization.IsNullOrWhiteSpace())
                return ResponseManager.BadRequest("Authoriazion Header required");

            if (!Authorized(request.Authorization))
                return ResponseManager.Unauthorized();

            var scoreBoard = new ScoresRepository().ScoreBoard();
            return ResponseManager.OK( JsonSerializer.Serialize(scoreBoard) );
        }
    }
}
