using MCTGClassLibrary.Database.Repositories;
using MCTGClassLibrary.DataObjects;
using MCTGClassLibrary.Networking.HTTP;
using System;
using System.IO;
using System.Text.Json;

namespace MCTGClassLibrary.Networking.EndpointHandlers
{
    public class Packages : EndpointHandlerBase
    {
        // post to packages => create package
        protected override Response PostHandler(Request request)
        {
            if (request.Payload.IsNullOrWhiteSpace())
                return ResponseManager.BadRequest("No Payload");

            if (!Authorized(request.Authorization))
                return ResponseManager.Unauthorized();

            string username = Session.GetUsername(ExtractAuthorizationToken(request.Authorization));
            if (!new UsersRepository().IsAdmin(username))
                return ResponseManager.Unauthorized("only admins can add new packages");

            CardData[] cardDataArray = JsonSerializer.Deserialize<CardData[]>(request.Payload);
            new PackagesRepository().AddPackage(cardDataArray);

            return ResponseManager.Created("Package added successfully");
        }
    }
}
