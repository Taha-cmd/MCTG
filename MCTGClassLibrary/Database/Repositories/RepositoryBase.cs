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

        protected void UpdateValue<FilterType, ValueType>(string table, string filter, FilterType filterValue, string columnToUpdate, ValueType newValue)
        {
            //update "user" set name = 'Taha' where username = 'taha';
            string statement = $"UPDATE \"{table}\" SET {columnToUpdate}=@newValue WHERE {filter}=@filterValue";

            database.ExecuteNonQuery(statement,
                                new NpgsqlParameter("newValue", newValue),
                                new NpgsqlParameter("filterValue", filterValue)
                            );
        }

        protected ValueType GetValue<ValueType, FilterType>(string table, string filter, FilterType filterValue, string columnToFetch, int? limit = null)
        {
            using var conn = database.GetConnection();
            string statement = $"SELECT {columnToFetch} FROM \"{table}\" WHERE {filter}=@filterValue";

            if (!limit.IsNull())
                statement += $" limit {limit}";

            using var command = new NpgsqlCommand(statement, conn);

            command.Parameters.AddWithValue("filterValue", filterValue);

            var reader = command.ExecuteReader();
            reader.Read();

            return reader.GetFieldValue<ValueType>(0);
        }
    }
}
