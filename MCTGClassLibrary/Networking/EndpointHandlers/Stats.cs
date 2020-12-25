using MCTGClassLibrary.Database.Repositories;
using MCTGClassLibrary.DataObjects;
using MCTGClassLibrary.Networking.HTTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace MCTGClassLibrary.Networking.EndpointHandlers
{
    public class Stats : EndpointHandlerBase
    {
        protected override Response GetHandler(Request request)
        {
            if (request.Authorization.IsNullOrWhiteSpace())
                return ResponseManager.BadRequest("Authoriazion Header required");

            try
            {
                if (!Authorized(request.Authorization))
                    return ResponseManager.Unauthorized();

                string username = ExtractUserNameFromAuthoriazionHeader(request.Authorization);
                if (!new UsersRepository().UserExists(username))
                    return ResponseManager.NotFound($"Username {username} does not exist");

                UserStats stats = new ScoresRepository().Stats(username);
                return ResponseManager.OK( JsonSerializer.Serialize(stats) );
            }
            catch (InvalidDataException ex)
            {
                return ResponseManager.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error from {ex.Source} in Stats GetHandler: {ex.Message}");
            }

            return ResponseManager.InternalServerError();
        }
    }
}
