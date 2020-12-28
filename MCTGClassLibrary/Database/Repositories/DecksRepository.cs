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
        public DecksRepository()
        {
            Table = "deck";
        } 
        public bool Empty(int id) => Count<int>(Table, "user_id", id) == 0;
        public bool Empty(string username) => Empty(new UsersRepository().GetUserID(username));
        public int Size(int id) => Count<int>(Table, "user_id", id);
        public int Size(string username) => Size(new UsersRepository().GetUserID(username));
        public CardData[] GetDeck(string username) => GetDeck(new UsersRepository().GetUserID(username));
        public void UpdateDeck(string username, params string[] cards) => UpdateDeck(new UsersRepository().GetUserID(username), cards);

        public CardData[] GetDeck(int id)
        {
            if (Empty(id))
                throw new InvalidDataException("the deck is empty");

            string statement = $"SELECT * FROM CARD JOIN {Table} ON CARD.id = {Table}.card_id WHERE user_id = @id";

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
                        Id = reader.GetValue<string>("id"),
                        Name = reader.GetValue<string>("name"),
                        Damage = reader.GetValue<double>("damage"),
                        Weakness = reader.GetValue<double>("weakness")
                    }
                ); 
            }

            return cards.ToArray();
        }


        // easiest solution for now: update all four references, create them if they don't exist
        public void UpdateDeck(int userID, params string[] cards)
        {
            var cardsRepo = new CardsRepository();

            foreach (string cardID in cards)
            {
                if (!cardsRepo.CardExists(cardID))
                    throw new InvalidDataException($"Card with ID {cardID} does not exist");
                
                if(!cardsRepo.InStack(userID, cardID))
                    throw new InvalidDataException($"Card with ID {cardID} is not in your stack");
            }


            int cardsToInsert = Config.DECKSIZE - Size(userID);
            int cardsToUpdate = 1 - cardsToInsert;
            int index = 0;

            for(; index < cardsToInsert; index++)
                InsertRecord(userID, cards[index]);

            // consider refactoring this ugly shit
            for (; index < cardsToUpdate; index++)
            {
                var currentDeck = GetDeck(userID);

                foreach (var card in currentDeck)
                    if (!card.Id.In(cards))
                        UpdateValue<string, string>(Table, "card_id", card.Id, "card_id", cards[index]);
            }
        }

        private void InsertRecord(int userID, string cardID)
        {
            string statement = $"INSERT INTO \"{Table}\" (user_id, card_id) VALUES(@user_id, @card_id)";
            database.ExecuteNonQuery(statement, new NpgsqlParameter("user_id", userID), new NpgsqlParameter("card_id", cardID));
        }

    }
}
