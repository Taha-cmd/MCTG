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

            UserData user = JsonSerializer.Deserialize<UserData>(request.Payload);

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

            if (request.RouteTokens.Length != 2)
                return ResponseManager.BadRequest("Invalid format");

            string requestedUsername = request.RouteTokens[1];
            if (requestedUsername != Session.GetUsername( ExtractAuthorizationToken(request.Authorization)) )
                return ResponseManager.Unauthorized($"you are not {requestedUsername}");

            UsersRepository usersRepo = new UsersRepository();
            if (!usersRepo.UserExists(requestedUsername))
                return ResponseManager.NotFound($"Username {requestedUsername} does not exist");

            UserData user = usersRepo.GetUser(requestedUsername);
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

            if (request.RouteTokens.Length != 2)
                return ResponseManager.BadRequest("Invalid format");

            string requestedUsername = request.RouteTokens[1];
            if (requestedUsername != Session.GetUsername(ExtractAuthorizationToken(request.Authorization)))
                return ResponseManager.Unauthorized($"you are not {requestedUsername}");

            UsersRepository usersRepo = new UsersRepository();

            if (!usersRepo.UserExists(requestedUsername))
                return ResponseManager.BadRequest($"Username {requestedUsername} does not exist");

            UserData user = JsonSerializer.Deserialize<UserData>(request.Payload);

            usersRepo.UpdateUser(requestedUsername, user);
            return ResponseManager.OK($"info for user {requestedUsername} updated successfully");

        }
    }
}
