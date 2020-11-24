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

            try
            {
                CardData[] cardDataArray;

                try
                {
                    cardDataArray = JsonSerializer.Deserialize<CardData[]>(request.Payload);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in PostHandler in Packages: " + ex.Message);
                    return ResponseManager.BadRequest("Invalid Json Format");
                }

                PackagesRepository packages = new PackagesRepository();
                packages.AddPackage(cardDataArray);

                return ResponseManager.Created("Package added successfully");
            }
            catch(InvalidDataException ex)
            {
                return ResponseManager.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in PostHandler in Packages: " + ex.Message);
            }

            return ResponseManager.InternalServerError();
        }
    }
}
