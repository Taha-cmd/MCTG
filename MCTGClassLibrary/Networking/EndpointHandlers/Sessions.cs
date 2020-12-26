using MCTGClassLibrary.Database.Repositories;
using MCTGClassLibrary.DataObjects;
using MCTGClassLibrary.Networking.HTTP;
using System;
using System.IO;
using System.Text.Json;

namespace MCTGClassLibrary.Networking.EndpointHandlers
{
    public class Sessions : EndpointHandlerBase
    {
        // POST to sessions => login => generate (for simplicity a static) token
        protected override Response PostHandler(Request request)
        {

            if (request.Payload.IsNullOrWhiteSpace())
                return ResponseManager.BadRequest("no payload");

            UserData user = JsonSerializer.Deserialize<UserData>(request.Payload);

            if (user.Username.IsNullOrWhiteSpace() || user.Password.IsNullOrWhiteSpace())
                return ResponseManager.BadRequest("empty username or password");

            var users = new UsersRepository();

            return users.Verify(user) ? ResponseManager.OK(GenerateToken(user.Username)) : ResponseManager.Unauthorized();

        }

        private string GenerateToken(string username)
        {
            return username + "-mtcgToken";
        }
    }
}
