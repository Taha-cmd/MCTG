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

            CardData[] cardDataArray = JsonSerializer.Deserialize<CardData[]>(request.Payload);

            PackagesRepository packages = new PackagesRepository();
            packages.AddPackage(cardDataArray);

            return ResponseManager.Created("Package added successfully");
        }
    }
}
