using MCTGClassLibrary.Database.Repositories;
using MCTGClassLibrary.DataObjects;
using MCTGClassLibrary.Networking.HTTP;
using System;
using System.IO;
using System.Text.Json;

namespace MCTGClassLibrary.Networking.EndpointHandlers
{
    public class Users : EndpointHandlerBase
    {

        // POST to users => sign up
        protected override Response PostHandler(Request request)
        {
            if (request.Payload.IsNullOrWhiteSpace())
                return ResponseManager.BadRequest("No payload");

            UserData user = user = JsonSerializer.Deserialize<UserData>(request.Payload);

            if (user.Username.IsNullOrWhiteSpace() || user.Password.IsNullOrWhiteSpace())
                return ResponseManager.BadRequest("Username or Password Empty");

            UsersRepository users = new UsersRepository();

            if (users.UserExists(user.Username))
                return ResponseManager.BadRequest($"Username {user.Username} allready exists");

            if (!users.RegisterUser(user))
                return ResponseManager.InternalServerError();

            return ResponseManager.Created($"Account for {user.Username} created successfully");
        }


        // GET to users => read user's data
        protected override Response GetHandler(Request request)
        {
            if (request.Authorization.IsNullOrWhiteSpace())
                return ResponseManager.BadRequest("Authoriazion Header required");

            if (!Authorized(request.Authorization))
                return ResponseManager.Unauthorized();

            string username = GetUserNameFromRoute(request.Route);
            UsersRepository usersRepo = new UsersRepository();

            if (!usersRepo.UserExists(username))
                return ResponseManager.NotFound($"Username {username} does not exist");

            UserData user = usersRepo.GetUser(username);
            return ResponseManager.OK( JsonSerializer.Serialize<UserData>(user) );
        }

        // PUT to users => update user's info
        protected override Response PutHandler(Request request)
        {
            if (request.Authorization.IsNullOrWhiteSpace())
                return ResponseManager.BadRequest("Authoriazion Header required");

            if (request.Payload.IsNullOrWhiteSpace())
                return ResponseManager.BadRequest("No Payload");

            if (!Authorized(request.Authorization))
                return ResponseManager.Unauthorized();

            string username = GetUserNameFromRoute(request.Route);
            UsersRepository usersRepo = new UsersRepository();

            if (!usersRepo.UserExists(username))
                return ResponseManager.BadRequest($"Username {username} does not exist");

            UserData user = JsonSerializer.Deserialize<UserData>(request.Payload);

            usersRepo.UpdateUser(username, user);
            return ResponseManager.OK("user's info updated successfully");

        }

        // helper methods
        private string GetUserNameFromRoute(string route)
        {
            try
            {
                route = route.Contains("?") ? route.Substring(0, route.IndexOf("?")) : route;
                string[] tokens = route.Split("/");
                route = tokens[2];
            }
            catch (Exception)
            {
                throw new InvalidDataException("Invalid request to this endpoint. Could not extract username");
            }

            return route;
        }
    }
}
