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
        public bool RegisterUser(string username, string password)
        {
            if (UserExists(username))
                return false;

            string statement = "INSERT INTO \"user\" (username, password) VALUES (@username, @password)";

            int rowsAffected = database.ExecuteNonQuery(
                    statement,
                    new NpgsqlParameter<string>("username", username),
                    new NpgsqlParameter<string>("password", password)
                );

            return rowsAffected == 1;
        }

        public bool UserExists(string username)
        {
            return Exists("user", "username", username);
        }

        public Card[] GetDeck(int userId)
        {
            throw new NotImplementedException();
        }

        public CardData[] GetStack(int userId)
        {
           return new CardsRepository().GetCards(userId);
        }

        public CardData[] GetStack(string username)
        {
            int id = GetUserID(username);

            if (id == -1)
                return new CardData[0];

            return new CardsRepository().GetCards(id);
        }

        public int GetUserID(string username)
        {
            using var conn = database.GetConnection();
            using var command = new NpgsqlCommand("SELECT id FROM \"user\" WHERE username=@username", conn);

            command.Parameters.AddWithValue("username", username);

            var reader = command.ExecuteReader();
            int id = -1;

            if (reader.Read())
                id = reader.GetInt32(reader.GetOrdinal("id"));

            return id;
        }


    }
}
