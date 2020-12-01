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

            try
            {
                UserData user;

                try
                {
                    user = JsonSerializer.Deserialize<UserData>(request.Payload);
                }
                catch(Exception ex)
                {
                    return ResponseManager.BadRequest("Invalid json");
                }

                if (user.Username.IsNullOrWhiteSpace() || user.Password.IsNullOrWhiteSpace())
                    return ResponseManager.BadRequest("empty username or password");

                var users = new UsersRepository();

                return users.Verify(user) ? ResponseManager.OK(GenerateToken(user.Username)) : ResponseManager.Unauthorized();
            }
            catch(InvalidDataException ex)
            {
                return ResponseManager.BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in Sessions PostHandler: " + ex.Message);
            }

            return ResponseManager.InternalServerError();
        }

        private string GenerateToken(string username)
        {
            return username + "-mtcgToken";
        }
    }
}
