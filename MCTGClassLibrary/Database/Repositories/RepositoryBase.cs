using Npgsql;

namespace MCTGClassLibrary.Database.Repositories
{
    public class RepositoryBase
    {
        protected Database database;
        public string Table { get; protected set; }
        protected RepositoryBase()
        {
            database = new Database(Config.HOST, Config.PORT, Config.DATABASE, Config.USERNAME, Config.PASSWORD);
        }

        protected void UpdateValue<FilterType, ValueType>(string table, string filter, FilterType filterValue, string columnToUpdate, ValueType newValue, string filterOperator = "=")
        {
            //update "user" set name = 'Taha' where username = 'taha';
            string statement = $"UPDATE \"{table}\" SET {columnToUpdate}=@newValue WHERE {filter} {filterOperator} @filterValue";

            database.ExecuteNonQuery(statement,
                                new NpgsqlParameter("newValue", newValue),
                                new NpgsqlParameter("filterValue", filterValue)
                            );
        }

        protected ValueType GetValue<ValueType, FilterType>(string table, string filter, FilterType filterValue, string columnToFetch, int? limit = null, string filterOperator = "=", string? orderByColumn = null)
        {
            using var conn = database.GetConnection();
            string statement = $"SELECT {columnToFetch} FROM \"{table}\" WHERE {filter} {filterOperator} @filterValue";

            statement += !orderByColumn.IsNull() ? $" order by {orderByColumn}" : "";
            statement += !limit.IsNull() ? $" limit {limit}" : "";

            using var command = new NpgsqlCommand(statement, conn);

            command.Parameters.AddWithValue("filterValue", filterValue);

            var reader = command.ExecuteReader();
            reader.Read();

            return reader.GetFieldValue<ValueType>(0);
        }

        protected void DeleteValue<FilterType>(string table, string filter, FilterType filerValue, string filterOperator = "=")
        {
            string statement = $"DELETE FROM \"{table}\" WHERE {filter} {filterOperator} @filterValue";
            database.ExecuteNonQuery(statement, new NpgsqlParameter("filterValue", filerValue));

        }
        protected int Count<FilterType>(string table, string filter, FilterType filterValue, string filterOperator = "=")
        {
            return GetValue<int, FilterType>(table, filter, filterValue, "COUNT(*)", null, filterOperator);
        }

        protected bool Exists<FilterType>(string table, string filter, FilterType filterValue)
        {
            return Count<FilterType>(table, filter, filterValue) == 1;
        }
    }
}
