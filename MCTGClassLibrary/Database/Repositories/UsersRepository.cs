﻿using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCTGClassLibrary.Database.Repositories
{
    public class UsersRepository
    {
        public UsersRepository() { }

        Database database = new Database(Config.HOST, Config.PORT, Config.DATABASE, Config.USERNAME, Config.PASSWORD);
        public bool RegisterUser(string username, string password)
        {
            using(var conn = database.GetConnection())
            {
                using(var command = new NpgsqlCommand("INSERT INTO \"user\" (username, password) VALUES (@username, @password)", conn))
                {
                    command.Parameters.AddWithValue("username", username);
                    command.Parameters.AddWithValue("password", password);

                    int rowsEffected = command.ExecuteNonQuery();

                    return rowsEffected == 1;
                }
            }
        }

        public bool UserExists(string username)
        {
            bool exists = true;

            using(var conn = database.GetConnection())
            {
                using(var command = new NpgsqlCommand("SELECT COUNT(*) FROM \"user\" WHERE username=@username", conn))
                {
                    command.Parameters.AddWithValue("username", username);

                    var reader = command.ExecuteReader();
                    if (reader.Read())
                        exists = reader.GetInt32(0) == 1;

                    return exists;
                }
            }
        }
    }
}
