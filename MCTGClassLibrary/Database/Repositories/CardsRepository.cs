using MCTGClassLibrary.Cards;
using MCTGClassLibrary.DataObjects;
using MCTGClassLibrary.Enums;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace MCTGClassLibrary.Database.Repositories
{
    public class CardsRepository : RepositoryBase
    {
        public CardsRepository()
        {
            Table = "card";
        }

        public bool AddCard(CardData cardData, int packageId)
        {
            if (CardExists(cardData.Id))
                return false;

            string statement = $"INSERT INTO \"{Table}\" (id, name, damage, weakness, element, card_type, package_id) VALUES" +
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

        public bool CardExists(string id) => Exists(Table, "id", id);

        public CardData GetCard(string cardID)
        {
            if (!CardExists(cardID))
                throw new InvalidDataException($"card {cardID} does not exist");

            using var conn = database.GetConnection();
            using var command = new NpgsqlCommand($"SELECT * FROM \"{Table}\" where \"{Table}\".id = @cardID", conn);
            command.Parameters.AddWithValue("cardID", cardID);

            var reader = command.ExecuteReader();
            reader.Read();

            return new CardData
            {
                Id = reader.GetValue<string>("id"),
                Name = reader.GetValue<string>("name"),
                Damage = reader.GetValue<double>("damage"),
                Weakness = reader.GetValue<double>("weakness")
            };
        }

        public CardData[] GetCards(string username) => GetCards(new UsersRepository().GetUserID(username));
        public CardData[] GetCards(int userId = -1)
        {
            List<CardData> cards = new List<CardData>();

            string statement = $"SELECT * FROM \"{Table}\" WHERE ";

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
                            reader.GetValue<string>("id"),
                            reader.GetValue<string>("name"),
                            reader.GetValue<double>("damage"),
                            reader.GetValue<double>("weakness")
                    ));
            }

            return cards.ToArray();
        }

        public bool InStack(int userID, string cardID) => GetCards(userID).Any(card => card.Id == cardID);
        public bool InStack(string username, string cardID) => GetCards(username).Any(card => card.Id == cardID);

    }
}
