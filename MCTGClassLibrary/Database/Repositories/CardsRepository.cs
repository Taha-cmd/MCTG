﻿using MCTGClassLibrary.DataObjects;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MCTGClassLibrary.Database.Repositories
{
    public class CardsRepository : RepositoryBase
    {
        public CardsRepository() { }

        public bool AddCard(CardData cardData)
        {
            if (CardExists(cardData.Id))
                return false;

            string statement = "INSERT INTO \"card\" (id, name, damage, weakness) VALUES (@id, @name, @damage, @weakness)";

            int rowsAffected = database.ExecuteNonQuery (
                                    statement,
                                    new NpgsqlParameter<string>("id", cardData.Id),
                                    new NpgsqlParameter<string>("name", cardData.Name),
                                    new NpgsqlParameter<double>("damage", cardData.Damage),
                                    new NpgsqlParameter<double>("weakness", cardData.Weakness)
                                );

            return rowsAffected == 1;

        }

        public int AddCards(params CardData[] cards)
        {
            int addedCards = 0;

            foreach (var cardData in cards)
                if (AddCard(cardData))
                    addedCards++;

            return addedCards;
        }

        public bool CardExists(string id)
        {
            return Exists("card", "id", id);
        }

        public CardData[] GetCards(int userId = -1)
        {
            List<CardData> cards = new List<CardData>();

            string statement = "SELECT * FROM \"card\" WHERE ";

            if (userId == -1)
                statement += "owner_id IS NULL";
            else
                statement += "owner_id=@owner_id";

            using var conn = database.GetConnection();
            using var command = new NpgsqlCommand(statement, conn);

            if (userId != -1)
                command.Parameters.AddWithValue("owner_id", userId);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                cards.Add(new CardData(
                            reader.GetFieldValue<string>(reader.GetOrdinal("id")),
                            reader.GetFieldValue<string>(reader.GetOrdinal("name")),
                            reader.GetFieldValue<double>(reader.GetOrdinal("damage")),
                            reader.GetFieldValue<double>(reader.GetOrdinal("weakness"))
                    ));
            }


            return cards.ToArray();
        }

    }
}