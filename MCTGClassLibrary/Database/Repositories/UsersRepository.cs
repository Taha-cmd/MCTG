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
        public UsersRepository()
        {
            Table = "user";
        }

        public int GetUserID(string username)
        {
            if (!UserExists(username))
                throw new InvalidDataException($"username {username} does not exist");

            return GetValue<int, string>(Table, "username", username, "id");
        }

        public string GetUsername(int id)
        {
            if (!UserExists(id))
                throw new InvalidDataException($"user with id {id} does not exist");

            return GetValue<string, int>(Table, "id", id, "username");
        }

        // for now just assume that username "admin" is the admin
        // still good to have the method, in case I implement an actual admin functionality, only this method has to change
        public bool IsAdmin(string username) => username.ToLower().Trim() == "admin";

        public bool UserExists(string username) => Exists(Table, "username", username);
        public bool UserExists(int id) => Exists(Table, "id", id);
        public int Coins(int id) => GetValue<int, int>(Table, "id", id, "coins");
        public int Coins(string username) => GetValue<int, string>(Table, "username", username, "coins");
        public void IncrementCoins(int id, int amount) => UpdateValue<int, int>(Table, "id", id, "coins", Coins(id) + amount);
        public void IncrementCoins(string username, int amount) => UpdateValue<string, int>(Table, "username", username, "coins", Coins(username) + amount);
        public CardData[] GetStack(int userId) => new CardsRepository().GetCards(userId);
        public CardData[] GetStack(string username) => new CardsRepository().GetCards(GetUserID(username));
        public UserData GetUser(string username) => GetUser(GetUserID(username));
        public CardData[] GetDeck(int userId) => new DecksRepository().GetDeck(userId);
        public CardData[] GetDeck(string username) => new DecksRepository().GetDeck(username);
        private string GetPassword(int id) => GetValue<string, int>(Table, "id", id, "password");
        private string GetPassword(string username) => GetValue<string, string>(Table, "username", username, "password");
        public bool Verify(UserData user) => UserExists(user.Username) && GetPassword(user.Username) == user.Password;
        public bool Verify(string username, string password) => UserExists(username) && GetPassword(username) == password;

        public bool RegisterUser(UserData user)
        {
            if (UserExists(user.Username))
                throw new InvalidDataException("User allready exists");

            string statement = $"INSERT INTO \"{Table}\" (username, password, coins, name, image, bio) " +
                               "VALUES (@username, @password, @coins, @name, @image, @bio)";

            var name    = user.Name.IsNull() ? new NpgsqlParameter("name", DBNull.Value) : new NpgsqlParameter<string>("name", user.Name);
            var image   = user.Image.IsNull() ? new NpgsqlParameter("image", DBNull.Value) : new NpgsqlParameter<string>("image", user.Image);
            var bio     = user.Bio.IsNull() ? new NpgsqlParameter("bio", DBNull.Value) : new NpgsqlParameter<string>("bio", user.Bio);

            int rowsAffected = database.ExecuteNonQuery(
                    statement,
                    new NpgsqlParameter<string>("username", user.Username),
                    new NpgsqlParameter<string>("password", user.Password),
                    new NpgsqlParameter<int>("coins", Config.COINS),
                    name,
                    image,
                    bio
                );

            new ScoresRepository().MakeEntry(GetUserID(user.Username));
            return rowsAffected == 1;
        }

        public UserData GetUser(int id)
        {
            if (!UserExists(id))
                throw new InvalidDataException("User doesn't exist");

            using var conn = database.GetConnection();
            using var command = new NpgsqlCommand($"SELECT * FROM \"{Table}\" WHERE id=@id", conn);

            command.Parameters.AddWithValue("id", id);

            var reader = command.ExecuteReader();
            UserData user = new UserData();

            if(reader.Read())
            {
                user.Username = reader.GetValue<string>("username");
                user.Name = reader.IsDBNull(reader.GetOrdinal("name")) ? null : reader.GetValue<string>("name");
                user.Bio = reader.IsDBNull(reader.GetOrdinal("bio")) ? null : reader.GetValue<string>("bio"); 
                user.Coins = reader.GetValue<int>("coins");
                user.Password = "top secret";
                user.Image = reader.IsDBNull(reader.GetOrdinal("image")) ? null : reader.GetValue<string>("image");

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

            if(!user.Username.IsNull() && UserExists(username))
                throw new InvalidDataException($"new username {username} allready exists");

            //TODO: implement iterator for UserData
            if ( !user.Username.IsNull() )  UpdateValue(Table, "username", username, "username", user.Username);
            if ( !user.Name.IsNull() )      UpdateValue(Table, "username", username, "name", user.Name);
            if ( !user.Password.IsNull() )  UpdateValue(Table, "username", username, "password", user.Password);
            if ( !user.Bio.IsNull() )       UpdateValue(Table, "username", username, "bio", user.Bio);
            if ( !user.Image.IsNull() )     UpdateValue(Table, "username", username, "image", user.Image);
        }
    }
}
