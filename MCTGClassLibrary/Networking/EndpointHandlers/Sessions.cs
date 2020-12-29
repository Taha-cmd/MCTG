using MCTGClassLibrary.Database.Repositories;
using MCTGClassLibrary.DataObjects;
using MCTGClassLibrary.Networking.HTTP;
using System;
using System.Linq;
using System.Text.Json;

namespace MCTGClassLibrary.Networking.EndpointHandlers
{
    public class Sessions : EndpointHandlerBase
    {
        // POST to sessions => login => generate token and save it in the session
        protected override Response PostHandler(Request request)
        {
            if (request.Payload.IsNullOrWhiteSpace())
                return ResponseManager.BadRequest("no payload");

            UserData user = JsonSerializer.Deserialize<UserData>(request.Payload);

            if (user.Username.IsNullOrWhiteSpace() || user.Password.IsNullOrWhiteSpace())
                return ResponseManager.BadRequest("empty username or password");

            bool authorized = new UsersRepository().Verify(user);

            if(authorized)
            {
                if (Session.HasSession(user.Username))
                    return ResponseManager.OK($"{user.Username} allready has an active session");

                string token = GenerateToken(user.Username);
                Session.CreateSession(user.Username, token);
                return ResponseManager.OK(token);
            }

            return ResponseManager.NotFound("Invalid Credentials");

        }

        // Delete to Sessions => logout
        protected override Response DeleteHandler(Request request)
        {
            if (request.Authorization.IsNullOrWhiteSpace())
                return ResponseManager.BadRequest("Authoriazion Header required");

            if (!Authorized(request.Authorization))
                return ResponseManager.Unauthorized("you don't have an active session");

            string username = Session.GetUsername( ExtractAuthorizationToken(request.Authorization) );
            Session.EndSession(username);

            return ResponseManager.OK($"Session for {username} successfully terminated");
        }

        //username not needed anymore
        private string GenerateToken(string username = null) => RandomString(64);
        //private string GenerateToken(string username) => username + "-mtcgToken";

        private string RandomString(int length)
        {
            // credits: https://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings
            var random = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
