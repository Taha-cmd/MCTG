using MCTGClassLibrary.DataObjects;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MCTGClassLibrary.Database.Repositories
{
    public class DecksRepository : RepositoryBase
    {
        public bool Empty(int id) => Count<int>("deck", "user_id", id) == 0;
        public bool Empty(string username) => Empty(new UsersRepository().GetUserID(username));
        public int Size(int id) => Count<int>("deck", "user_id", id);
        public int Size(string username) => Size(new UsersRepository().GetUserID(username));
        public CardData[] GetDeck(string username) => GetDeck(new UsersRepository().GetUserID(username));

        public CardData[] GetDeck(int id)
        {
            if (Empty(id))
                throw new InvalidDataException("the deck is empty");

            string statement = "SELECT * FROM CARD JOIN DECK ON CARD.id = DECK.card_id WHERE user_id = @id";

            using var conn = database.GetConnection();
            using var command = new NpgsqlCommand(statement, conn);
            command.Parameters.AddWithValue("id", id);

            List<CardData> cards = new List<CardData>();

            var reader = command.ExecuteReader();

            while(reader.Read())
            {
                cards.Add
                (
                    new CardData()
                    {
                        Id = reader.GetString(reader.GetOrdinal("id")),
                        Name = reader.GetString(reader.GetOrdinal("name")),
                        Damage = reader.GetDouble(reader.GetOrdinal("damage")),
                        Weakness = reader.GetDouble(reader.GetOrdinal("weakness"))
                    }
                ); 
            }

            return cards.ToArray();
        }
    }
}
