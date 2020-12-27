using MCTGClassLibrary.DataObjects;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MCTGClassLibrary.Database.Repositories
{
    public class ScoresRepository : RepositoryBase
    {
        public ScoresRepository()
        {
            Table = "scoreboard";
        }

        public void MakeEntry(int userID) => database.ExecuteNonQuery($"INSERT INTO \"{Table}\" (user_id) VALUES (@user_id)", new NpgsqlParameter("user_id", userID));
        public void MakeEntry(string username) => MakeEntry(new UsersRepository().GetUserID(username));

        
        public int GetScore(int userID) => GetValue<int, int>(Table, "user_id", userID, "points");
        public int GetScore(string username) => GetScore(new UsersRepository().GetUserID(username));

        public int BattlesCount(int userID) => GetValue<int, int>(Table, "user_id", userID, "battles");
        public int BattlesCount(string username) => BattlesCount(new UsersRepository().GetUserID(username));

        public int WonBattlesCount(int userID) => GetValue<int, int>(Table, "user_id", userID, "won_battles");
        public int WonBattlesCount(string username) => WonBattlesCount(new UsersRepository().GetUserID(username));

        public int LostBattlesCount(int userID) => GetValue<int, int>(Table, "user_id", userID, "lost_battles");
        public int LostBattlesCount(string username) => LostBattlesCount(new UsersRepository().GetUserID(username));

        public void IncreaseBattles(int userID, int amount = 1) => UpdateValue<int, int>(Table, "user_id", userID, "battles", BattlesCount(userID) + amount);
        public void IncreaseBattles(string username, int amount = 1) => IncreaseBattles(new UsersRepository().GetUserID(username), amount);

        public void IncreaseScore(int userID, int amount) => UpdateValue<int, int>(Table, "user_id", userID, "points", GetScore(userID) + amount);
        public void IncreaseScore(string username, int amount) => IncreaseScore(new UsersRepository().GetUserID(username), amount);

        public void WonBattle(int userID) => UpdateValue<int, int>(Table, "user_id", userID, "won_battles", WonBattlesCount(userID) + 1);
        public void WonBattle(string username) => WonBattle(new UsersRepository().GetUserID(username));

        public void LostBattle(int userID) => UpdateValue<int, int>(Table, "user_id", userID, "lost_battles", LostBattlesCount(userID) + 1);
        public void LostBattle(string username) => LostBattle(new UsersRepository().GetUserID(username));


        public UserStats Stats(string username) => Stats(new UsersRepository().GetUserID(username));
        public UserStats Stats(int userID)
        {
            if (!new UsersRepository().UserExists(userID))
                throw new InvalidDataException($"User with id {userID} does not exist");

            string statement = $"SELECT * FROM \"{Table}\" WHERE user_id = @user_id";
            using var conn = database.GetConnection();
            using var command = new NpgsqlCommand(statement, conn);
            command.Parameters.AddWithValue("user_id", userID);

            UserStats stats = new UserStats 
            { 
                UserId = userID,
                Username = new UsersRepository().GetUsername(userID)
            };

            var reader = command.ExecuteReader();

            if(reader.Read())
            {
                stats.Points = reader.GetValue<int>("points");
                stats.Battles = reader.GetValue<int>("battles");
                stats.WonBattles = reader.GetValue<int>("won_battles");
                stats.LostBattles = reader.GetValue<int>("lost_battles");
            }

            return stats;
        }

        public List<UserStats> ScoreBoard()
        {
            using var conn = database.GetConnection();
            using var command = new NpgsqlCommand($"SELECT * FROM \"{Table}\" JOIN \"user\" ON \"{Table}\".user_id = \"user\".id ORDER BY \"points\" DESC", conn);
            var reader = command.ExecuteReader();

            List<UserStats> scoreBoard = new List<UserStats>();

            while(reader.Read())
            {
                scoreBoard.Add
                (
                    new UserStats
                    {
                        UserId = reader.GetValue<int>("user_id"),
                        Username = reader.GetValue<string>("username"),
                        Points = reader.GetValue<int>("points"),
                        Battles = reader.GetValue<int>("battles"),
                        WonBattles = reader.GetValue<int>("won_battles"),
                        LostBattles = reader.GetValue<int>("lost_battles")
                    }
                );
            }

            return scoreBoard;
        }
    }
}
