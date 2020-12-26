using MCTGClassLibrary.Database.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MCTGClassLibrary.Networking.HTTP;

namespace MCTGClassLibrary.Networking.EndpointHandlers
{
    public class Transactions : EndpointHandlerBase
    {
        // post to transactions/packages => acquire package
        protected override Response PostHandler(Request request)
        {

            if (request.Authorization.IsNullOrWhiteSpace())
                return ResponseManager.BadRequest("Authoriazion Header required");

            if (!Authorized(request.Authorization))
                return ResponseManager.Unauthorized("Authorization Failed!, check your username and password");

            string username = ExtractUserNameFromAuthoriazionHeader(request.Authorization);
            string destination = GetNthTokenFromRoute(2, request.Route);

            switch (destination.ToLower())
            {
                case "packages": AcquirePackage(username); break;
            }

            return ResponseManager.OK("success");
        }


        private void AcquirePackage(string username)
        {
            var users = new UsersRepository();

            if (!users.UserExists(username))
                throw new InvalidDataException($"User {username} does not exist");

            int coins = users.Coins(username);
            if (coins< Config.PACKAGECOST)
                throw new InvalidDataException($"Not enough coins to buy a package. current coins: {coins}");

            var packages = new PackagesRepository();
            if (packages.AvailablePackages() < 1)
                throw new InvalidDataException("No packages available");

            int packageId = packages.NextPackage();

            packages.TransferOwnership(packageId, username);
            packages.SetAvailability(packageId, false);
            users.IncrementCoins(username, -Config.PACKAGECOST);
        }
    }
}
