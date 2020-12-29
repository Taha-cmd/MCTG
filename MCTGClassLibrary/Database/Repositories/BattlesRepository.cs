using Npgsql;
using System;

namespace MCTGClassLibrary.Database.Repositories
{
    public class BattlesRepository : RepositoryBase
    {
        public BattlesRepository()
        {
            Table = "battle";
        }

        public void AddBattle(string player1, string player2, string? winner, string log, int rounds)
        {
            var users = new UsersRepository();

            int? winnerID = null;
            if (!winner.IsNull())
                winnerID = users.GetUserID(winner);

            AddBattle(users.GetUserID(player1), users.GetUserID(player2), winnerID, log, rounds);
        }

        public void AddBattle(int player1id, int player2id, int? winnerId, string log, int rounds)
        {
            string statement = $"INSERT INTO \"{Table}\" (player1_id, player2_id, winner_id, log, rounds) VALUES (@player1_id, @player2_id, @winner_id, @log, @rounds)";

            database.ExecuteNonQuery(statement,
                    new NpgsqlParameter<int>("player1_id", player1id),
                    new NpgsqlParameter<int>("player2_id", player2id),
                    winnerId.IsNull() ? new NpgsqlParameter("winner_id", DBNull.Value) : new NpgsqlParameter("winner_id", winnerId),
                    new NpgsqlParameter("log", log),
                    new NpgsqlParameter("rounds", rounds)

                );
        }

    }
}
