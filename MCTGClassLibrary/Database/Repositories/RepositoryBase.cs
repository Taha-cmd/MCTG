using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCTGClassLibrary.Database.Repositories
{
    public class RepositoryBase
    {
        protected Database database;
        protected RepositoryBase()
        {
            database = new Database(Config.HOST, Config.PORT, Config.DATABASE, Config.USERNAME, Config.PASSWORD);
        }

        protected bool Exists<T>(string table, string filter, T value)
        {
            bool exists = true;

            using var conn = database.GetConnection();
            using var command = new NpgsqlCommand($"SELECT COUNT(*) FROM \"{table}\" WHERE {filter}=@value", conn);

            command.Parameters.AddWithValue("value", value);

            var reader = command.ExecuteReader();
            if (reader.Read())
                exists = reader.GetInt32(0) == 1;

            return exists;
        }
    }
}
