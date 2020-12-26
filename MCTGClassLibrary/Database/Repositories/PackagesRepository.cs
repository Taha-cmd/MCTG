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
        public PackagesRepository()
        {
            Table = "package";
        }
        public void AddPackage(params CardData[] cards)
        {
            if (cards.Length != Config.PACKAGESIZE)
                throw new InvalidDataException($"Error adding package: Package size must be {Config.PACKAGESIZE}");

            CardsRepository cardsRepo = new CardsRepository();

            foreach (var card in cards)
                if (cardsRepo.CardExists(card.Id))
                    throw new InvalidDataException($"Error adding Package: card with Id {card.Id} allready exists");

            int newPackageId = MakeNewPackageEntry();
            cardsRepo.AddCards(newPackageId, cards);

            //foreach (var card in cards)
              //  AddPackageCardReference(newPackageId, card.Id);
        }

        // buying packages is like a queue, first created first sold
        public int NextPackage()
        {
            if (AvailablePackages() < 1)
                throw new InvalidDataException("No packages available");

            return GetValue<int, int>(Table, "available", 1, "id", 1, "=", "creation_date");

        }

        public bool PackageExists(int id) => Exists(Table, "id", id);
        public int AvailablePackages() => Count<int>(Table, "available", 1);
        public void SetAvailability(int id, bool available) => UpdateValue<int, int>(Table, "id", id, "available", available ? 1 : 0);
        public void TransferOwnership(int packageId, int userId) => UpdateValue<int, int>("card", "package_id", packageId, "owner_id", userId);
        public void TransferOwnership(int packageId, string username) => TransferOwnership(packageId, new UsersRepository().GetUserID(username));

        private int MakeNewPackageEntry()
        {

            string statement = $"INSERT INTO \"{Table}\" DEFAULT VALUES";
            int rowsAffected = database.ExecuteNonQuery(statement);

            if (rowsAffected != 1)
                throw new NpgsqlException("Error creating package");

            // select max from a table
            // workaround: no condition needed => 1 = 1
            return GetValue<int, string>(Table, "\'1\'", "1", "MAX(\"id\")");
        }
    }
}
