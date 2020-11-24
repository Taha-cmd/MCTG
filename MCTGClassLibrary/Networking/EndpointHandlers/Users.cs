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

            try
            {
                UserData user;

                try
                {
                    user = JsonSerializer.Deserialize<UserData>(request.Payload);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error in PostHandler in Users: " + ex.Message);
                    return ResponseManager.BadRequest("Invalid Json Format");
                }

                if (user.Username.IsNullOrWhiteSpace() || user.Password.IsNullOrWhiteSpace())
                    return ResponseManager.BadRequest("Username or Password Empty");

                UsersRepository users = new UsersRepository();

                if(users.UserExists(user.Username))
                    return ResponseManager.BadRequest($"Username {user.Username} allready exists");

                if (users.RegisterUser(user))
                    return ResponseManager.Created($"Account for {user.Username} created successfully");
   
            }
            catch(InvalidDataException ex)
            {
                return ResponseManager.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in PostHandler in Users: " + ex.Message);
            }

            return ResponseManager.InternalServerError();
        }


        // GET to users => read user's data
        protected override Response GetHandler(Request request)
        {
            if (request.Authorization.IsNullOrWhiteSpace())
                return ResponseManager.BadRequest("Authoriazion Header required");

            try
            {
                if (!Authorized(request.Authorization))
                    return ResponseManager.Unauthorized();

                string username = GetUserNameFromRoute(request.Route);
                UsersRepository usersRepo = new UsersRepository();

                if (!usersRepo.UserExists(username))
                    return ResponseManager.NotFound($"Username {username} does not exist");

                UserData user = usersRepo.GetUser(username);
                string userInfo = JsonSerializer.Serialize<UserData>(user);
                return ResponseManager.OK(userInfo);
            }
            catch(InvalidDataException ex)
            {
                return ResponseManager.BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in Users GetHandler: " + ex.Message);
            }

            return ResponseManager.InternalServerError();
        }

        // PUT to users => update user's info
        protected override Response PutHandler(Request request)
        {
            if (request.Authorization.IsNullOrWhiteSpace())
                return ResponseManager.BadRequest("Authoriazion Header required");

            if (request.Payload.IsNullOrWhiteSpace())
                return ResponseManager.BadRequest("No Payload");

            try
            {
                if (!Authorized(request.Authorization))
                    return ResponseManager.Unauthorized();

                string username = GetUserNameFromRoute(request.Route);
                UsersRepository usersRepo = new UsersRepository();

                if (!usersRepo.UserExists(username))
                    return ResponseManager.BadRequest($"Username {username} does not exist");

                UserData user;

                try
                {
                    user = JsonSerializer.Deserialize<UserData>(request.Payload);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error parsing JSON in Users PutHandler: " + ex.Message);
                    return ResponseManager.BadRequest("Invalid JSON Format");
                }

                usersRepo.UpdateUser(username, user);
                return ResponseManager.OK("user's info updated successfully");
            }
            catch (InvalidDataException ex)
            {
                return ResponseManager.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error from {ex.Source} in Users PutHandler: " + ex.Message);
            }

            return ResponseManager.InternalServerError();
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
