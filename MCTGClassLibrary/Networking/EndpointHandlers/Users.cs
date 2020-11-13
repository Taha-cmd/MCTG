﻿using MCTGClassLibrary.Database.Repositories;
using MCTGClassLibrary.DataObjects;
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
            if (string.IsNullOrWhiteSpace(request.Payload))
                return new Response("No payload");

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
                    return new Response("Invalid Json Format");
                }

                if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
                    return new Response("Username or Password Empty");

                UsersRepository users = new UsersRepository();

                if(users.UserExists(user.Username))
                    return new Response($"Username {user.Username} allready exists");

                if (users.RegisterUser(user))
                    return new Response("200", "OK", $"Account for {user.Username} created successfully");
   
            }
            catch(InvalidDataException ex)
            {
                return new Response(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in PostHandler in Users: " + ex.Message);
            }

            return new Response("500", "Internal Server Error");
        }


        // GET to users => read user's data
        protected override Response GetHandler(Request request)
        {
            if (string.IsNullOrWhiteSpace(request.Authorization))
                return new Response("Authoriazion Header required");

            try
            {
                if (!Authorized(request.Authorization))
                    return new Response("This Action requires authorization");

                string username = GetUserNameFromRoute(request.Route);
                UsersRepository usersRepo = new UsersRepository();

                if (!usersRepo.UserExists(username))
                    return new Response($"Username {username} does not exist");

                UserData user = usersRepo.GetUser(username);
                string userInfo = JsonSerializer.Serialize<UserData>(user);
                return new Response("200", "OK", userInfo);
            }
            catch(InvalidDataException ex)
            {
                return new Response(ex.Message);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in Users GetHandler: " + ex.Message);
            }

            return new Response("500", "Internal Server Error");
        }

        // PUT to users => update user's info
        protected override Response PutHandler(Request request)
        {
            if (string.IsNullOrWhiteSpace(request.Authorization))
                return new Response("Authoriazion Header required");

            if (string.IsNullOrWhiteSpace(request.Payload))
                return new Response("No Payload");

            try
            {
                if (!Authorized(request.Authorization))
                    return new Response("This Action requires authorization");

                string username = GetUserNameFromRoute(request.Route);
                UsersRepository usersRepo = new UsersRepository();

                if (!usersRepo.UserExists(username))
                    return new Response($"Username {username} does not exist");

                UserData user;

                try
                {
                    user = JsonSerializer.Deserialize<UserData>(request.Payload);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error parsing JSON in Users PutHandler: " + ex.Message);
                    return new Response("Invalid JSON Format");
                }

                usersRepo.UpdateUser(username, user);
                return new Response("200", "OK", "user's info updated successfully");
            }
            catch (InvalidDataException ex)
            {
                return new Response(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error from {ex.Source} in Users PutHandler: " + ex.Message);
            }

            return new Response("500", "Internal Server Error");
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
