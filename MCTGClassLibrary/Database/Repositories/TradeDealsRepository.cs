using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MCTGClassLibrary.DataObjects;
using Npgsql;

namespace MCTGClassLibrary.Database.Repositories
{
    public class TradeDealsRepository : RepositoryBase
    {
        public TradeDealsRepository()
        {
            Table = "trade_deal";
        }
        public bool DealExists(string id) => Exists<string>(Table, "id", id);
        public void RemoveDeal(string dealID) => DeleteValue<string>(Table, "id", dealID);
        public bool HasDeal(int userID, string dealID) => DealExists(dealID) && GetValue<int, string>(Table, "id", dealID, "owner_id") == userID;
        public bool HasDeal(string username, string dealID) => HasDeal(new UsersRepository().GetUserID(username), dealID);
        public TradeDeal GetDeal(string id)
        {
            if (!DealExists(id))
                throw new InvalidDataException($"Trade deal {id} does not exist");

            using var conn = database.GetConnection();
            using var command = new NpgsqlCommand($"SELECT * FROM \"{Table}\" WHERE \"{Table}\".id = @id", conn);
            command.Parameters.AddWithValue("id", id);

            var reader = command.ExecuteReader();

            if (!reader.Read())
                throw new InvalidDataException("No trade deals available");

            return new TradeDeal
            {
                Id = reader.GetValue<string>("id"),
                OwnerId = reader.GetValue<int>("owner_id"),
                CardId = reader.GetValue<string>("card_id"),
                MinimumDamage = reader.GetValue<double>("min_damage"),
                MaximumWeakness = reader.GetValue<double>("max_weakness"),
                ElementType = reader.GetValue<string>("element"),
                CardType = reader.GetValue<string>("card_type")
            };
        }

        public TradeDeal[] GetDeals()
        {
            using var conn = database.GetConnection();
            using var command = new NpgsqlCommand($"SELECT * FROM \"{Table}\"", conn);
            var reader = command.ExecuteReader();

            var deals = new List<TradeDeal>();

            while (reader.Read())
            {
                deals.Add
                (
                    new TradeDeal
                    {
                        Id = reader.GetValue<string>("id"),
                        OwnerId = reader.GetValue<int>("owner_id"),
                        CardId = reader.GetValue<string>("card_id"),
                        MinimumDamage = reader.GetValue<double>("min_damage"),
                        MaximumWeakness = reader.GetValue<double>("max_weakness"),
                        ElementType = reader.GetValue<string>("element"),
                        CardType = reader.GetValue<string>("card_type")
                    }

                );
            }

            return deals.ToArray();
        }

        public void AddDeal(TradeDeal deal)
        {
            string statement = $"INSERT INTO \"{Table}\" (id, owner_id, card_id, min_damage, max_weakness, card_type, element) " +
                $"VALUES (@id, @owner_id, @card_id, @min_damage, @max_weakness, @card_type, @element)";

            var max_weakness = deal.MaximumWeakness.IsNull() ? new NpgsqlParameter("max_weakness", DBNull.Value) : new NpgsqlParameter("max_weakness", deal.MaximumWeakness);
            var card_type = deal.CardType.IsNull() ? new NpgsqlParameter("card_type", DBNull.Value) : new NpgsqlParameter("card_type", deal.CardType);
            var element = deal.ElementType.IsNull() ? new NpgsqlParameter("element", DBNull.Value) : new NpgsqlParameter("element", deal.ElementType);

            database.ExecuteNonQuery(statement,
                    new NpgsqlParameter("id", deal.Id),
                    new NpgsqlParameter("owner_id", deal.OwnerId),
                    new NpgsqlParameter("card_id", deal.CardId),
                    new NpgsqlParameter("min_damage", deal.MinimumDamage),
                    max_weakness,
                    card_type,
                    element
                );
        }
    }
}
