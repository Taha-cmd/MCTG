﻿using MCTGClassLibrary.Database.Repositories;
using MCTGClassLibrary.DataObjects;
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
                return new Response("No Payload");

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
                    return new Response("Invalid Json Format");
                }

                PackagesRepository packages = new PackagesRepository();
                packages.AddPackage(cardDataArray);

                return new Response("200", "OK", "Package added successfully");
            }
            catch(InvalidDataException ex)
            {
                return new Response(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in PostHandler in Packages: " + ex.Message);
            }

            return new Response("500", "Internal Server Error");
        }
    }
}
