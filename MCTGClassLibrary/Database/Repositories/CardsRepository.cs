using MCTGClassLibrary.Cards;
using MCTGClassLibrary.DataObjects;
using MCTGClassLibrary.Enums;
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

        public bool AddCard(CardData cardData, int packageId)
        {
            if (CardExists(cardData.Id))
                return false;

            string statement = "INSERT INTO \"card\" (id, name, damage, weakness, element, card_type, package_id) VALUES" +
                " (@id, @name, @damage, @weakness, @element, @card_type, @package_id)";

            string cardType = CardsManager.ExtractCardType(cardData.Name) == CardType.Spell ? "spell"
                              : CardsManager.ExtractMonsterType(cardData.Name).ToString().ToLower();

            int rowsAffected = database.ExecuteNonQuery(
                                    statement,
                                    new NpgsqlParameter<string>("id", cardData.Id),
                                    new NpgsqlParameter<string>("name", cardData.Name),
                                    new NpgsqlParameter<double>("damage", cardData.Damage),
                                    new NpgsqlParameter<double>("weakness", cardData.Weakness),
                                    new NpgsqlParameter<string>("element", CardsManager.ExtractElementType(cardData.Name).ToString().ToLower()),
                                    new NpgsqlParameter<string>("card_type", cardType),
                                    new NpgsqlParameter<int>("package_id", packageId)
                                ); ;

            return rowsAffected == 1;

        }

        public int AddCards(int newPackageId, params CardData[] cards)
        {
            int addedCards = 0;

            foreach (var cardData in cards)
                if (AddCard(cardData, newPackageId))
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
