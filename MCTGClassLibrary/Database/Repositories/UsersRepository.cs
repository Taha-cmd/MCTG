using MCTGClassLibrary.Cards;
using MCTGClassLibrary.DataObjects;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace MCTGClassLibrary.Database.Repositories
{
    public class UsersRepository : RepositoryBase
    {
        public UsersRepository() { }

        public int GetUserID(string username)
        {
            if (!UserExists(username))
                throw new InvalidDataException($"username {username} does not exist");

            return GetValue<int, string>("user", "username", username, "id");
        }

        public string GetUsername(int id)
        {
            if (!UserExists(id))
                throw new InvalidDataException($"user with id {id} does not exist");

            return GetValue<string, int>("user", "id", id, "username");
        }

        public bool UserExists(string username) => Exists("user", "username", username);
        public bool UserExists(int id) => Exists("user", "id", id);
        public int Coins(int id) => GetValue<int, int>("user", "id", id, "coins");
        public int Coins(string username) => GetValue<int, string>("user", "username", username, "coins");
        public void IncrementCoins(int id, int amount) => UpdateValue<int, int>("user", "id", id, "coins", Coins(id) + amount);
        public void IncrementCoins(string username, int amount) => UpdateValue<string, int>("user", "username", username, "coins", Coins(username) + amount);
        public CardData[] GetStack(int userId) => new CardsRepository().GetCards(userId);
        public CardData[] GetStack(string username) => new CardsRepository().GetCards(GetUserID(username));
        public UserData GetUser(string username) => GetUser(GetUserID(username));
        public CardData[] GetDeck(int userId) => new DecksRepository().GetDeck(userId);
        public CardData[] GetDeck(string username) => new DecksRepository().GetDeck(username);

        public bool RegisterUser(UserData user)
        {
            if (UserExists(user.Username))
                throw new InvalidDataException("User allready exists");

            string statement = "INSERT INTO \"user\" (username, password, coins, name, image, bio) " +
                               "VALUES (@username, @password, @coins, @name, @image, @bio)";

            var name    = user.Name == null ? new NpgsqlParameter("name", DBNull.Value) : new NpgsqlParameter<string>("name", user.Name);
            var image   = user.Image == null ? new NpgsqlParameter("image", DBNull.Value) : new NpgsqlParameter<string>("image", user.Image);
            var bio     = user.Bio == null ? new NpgsqlParameter("bio", DBNull.Value) : new NpgsqlParameter<string>("bio", user.Bio);

            int rowsAffected = database.ExecuteNonQuery(
                    statement,
                    new NpgsqlParameter<string>("username", user.Username),
                    new NpgsqlParameter<string>("password", user.Password),
                    new NpgsqlParameter<int>("coins", Config.COINS),
                    name,
                    image,
                    bio
                );

            return rowsAffected == 1;
        }

        public UserData GetUser(int id)
        {
            if (!UserExists(id))
                throw new InvalidDataException("User doesn't exist");

            using var conn = database.GetConnection();
            using var command = new NpgsqlCommand("SELECT * FROM \"user\" WHERE id=@id", conn);

            command.Parameters.AddWithValue("id", id);

            var reader = command.ExecuteReader();
            UserData user = new UserData();

            if(reader.Read())
            {
                user.Username = reader.GetString(reader.GetOrdinal("username"));
                user.Name = reader.IsDBNull(reader.GetOrdinal("name")) ? null : reader.GetString(reader.GetOrdinal("name"));
                user.Bio = reader.IsDBNull(reader.GetOrdinal("bio")) ? null : reader.GetString(reader.GetOrdinal("bio")); 
                user.Coins = reader.GetInt32(reader.GetOrdinal("coins"));
                user.Password = "top secret";
                user.Image = reader.IsDBNull(reader.GetOrdinal("image")) ? null : reader.GetString(reader.GetOrdinal("image"));

                // can assign null to strings
                // but can't assign null values from the db to strings
                // genius
            }

            return user;
        }

        public void UpdateUser(string username, UserData user)
        {
            if (!UserExists(username))
                throw new InvalidDataException($"User {username} does not exist");

            if(user.Username != null && UserExists(username))
                throw new InvalidDataException($"new username {username} allready exists");

            //TODO: implement iterator for UserData
            if (user.Username != null)  UpdateValue("user", "username", username, "username", user.Username);
            if (user.Name != null)      UpdateValue("user", "username", username, "name", user.Name);
            if (user.Password != null)  UpdateValue("user", "username", username, "password", user.Password);
            if (user.Bio != null)       UpdateValue("user", "username", username, "bio", user.Bio);
            if (user.Image != null)     UpdateValue("user", "username", username, "image", user.Image);
        }
    }
}
