using MCTGClassLibrary.Database.Repositories;
using MCTGClassLibrary.DataObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using MCTGClassLibrary.Networking.HTTP;

namespace MCTGClassLibrary.Networking.EndpointHandlers
{
    public class Cards : EndpointHandlerBase
    {

        // should return all cards of a user (the stack)
        protected override Response GetHandler(Request request)
        {
            if (request.Authorization.IsNullOrWhiteSpace())
                return ResponseManager.BadRequest("No Authorization Header found");

            if (!Authorized(request.Authorization))
                return ResponseManager.Unauthorized("Authorization Failed!, check your username and password");

            string username = Session.GetUsername(ExtractAuthorizationToken(request.Authorization));
            CardData[] cardDataArray = new UsersRepository().GetStack(username);

            if (cardDataArray.Length == 0)
                return ResponseManager.OK("Stack is empty");

            return ResponseManager.OK( JsonSerializer.Serialize<CardData[]>(cardDataArray) );

        }
    }
}