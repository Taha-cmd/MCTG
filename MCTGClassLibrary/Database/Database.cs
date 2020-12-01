using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Npgsql;

namespace MCTGClassLibrary.Database
{
    public class Database
    {
        public string Host { get; }
        public string Port { get; }
        public string DBname { get; }
        public string User { get; }
        public string Password { get; }

        private string connectionString;

        public Database(string host, string port, string db, string user, string password)
        {
            Host = host;
            Port = port;
            DBname = db;
            User = user;
            Password = password;

            connectionString = $"Server={Host};Username={User};Database={DBname};Port={Port};Password={Password};SSLMode=Prefer";
        }

        public NpgsqlConnection GetConnection()
        {
            var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            return conn;
        }

        public int ExecuteNonQuery(string statement, params NpgsqlParameter[] parameters)
        {
            using var conn = GetConnection();
            using var command = new NpgsqlCommand(statement, conn);

            command.Parameters.AddRange(parameters);

            return command.ExecuteNonQuery();
        }

    }
}
