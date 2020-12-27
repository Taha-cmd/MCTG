using System;
using System.Collections.Generic;
using System.Text;

namespace MCTGClassLibrary.Networking
{
    public static class Session
    {
        // map username to tokens
        // no expiration date for simplicity

        private static Dictionary<string, string> sessions = new Dictionary<string, string>();

        public static void CreateSession(string username, string token) => sessions.Add(username, token);
        public static bool HasSession(string username)                  => sessions.ContainsKey(username);
        public static void EndSession(string username)                  => sessions.Remove(username);
        public static string GetToken(string username)                  => sessions.ContainsKey(username) ? sessions[username] : null;
        public static bool TokenExists(string token)                    => sessions.ContainsValue(token);

        public static string GetUsername(string token)
        {
            // https://stackoverflow.com/questions/2444033/get-dictionary-key-by-value
            foreach (var kvp in sessions)
                if (kvp.Value == token)
                    return kvp.Key;

            return null;
        }
    }
}
