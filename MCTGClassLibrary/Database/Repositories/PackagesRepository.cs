using MCTGClassLibrary.DataObjects;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MCTGClassLibrary.Database.Repositories
{
    public class PackagesRepository : RepositoryBase
    {
        public void AddPackage(params CardData[] cards)
        {
            if (cards.Length != 5)
                throw new InvalidDataException("Error adding package: Package size must be 5");

            CardsRepository cardsRepo = new CardsRepository();

            foreach (var card in cards)
                if (cardsRepo.CardExists(card.Id))
                    throw new InvalidDataException($"Error adding Package: card with Id {card.Id} allready exists");

            cardsRepo.AddCards(cards);
            int newPackageId = MakeNewPackageEntry();

            foreach (var card in cards)
                AddPackageCardReference(newPackageId, card.Id);
        }

        public bool PackageExists(int id)
        {
            return Exists("package", "id", 1);
        }

        private int MakeNewPackageEntry()
        {

            string statement = "INSERT INTO \"package\" DEFAULT VALUES";
            int rowsAffected = database.ExecuteNonQuery(statement);

            if (rowsAffected != 1)
                throw new NpgsqlException("Error creating package");

            using var conn = database.GetConnection();
            using var command = new NpgsqlCommand("SELECT MAX(\"id\") FROM \"package\"", conn);

            var reader = command.ExecuteReader();

            if (!reader.Read())
                throw new NpgsqlException("Error reading max id of packages");

            return reader.GetInt32(0);
        }

        private void AddPackageCardReference(int packageId, string cardId)
        {
            string statement = "INSERT INTO \"package_card\" (package_id, card_id) VALUES(@package_id, @card_id)";

            int rowsAffected = database.ExecuteNonQuery(statement,
                                    new NpgsqlParameter("package_id", packageId),
                                    new NpgsqlParameter("card_id", cardId)
                               );

            if (rowsAffected != 1)
                throw new NpgsqlException("Error adding package_card reference");
        }
    }
}
