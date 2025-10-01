// UnbanMenuPlugin.cs
// CS2 / CounterStrikeSharp plugin skeleton with config-based DB settings.
// NOTE: Depending on your CSSharp version, you may need to adapt the API calls (Commands, Menu, Client).
// This project is intended as a starting point. Adjust namespaces/types to match your CounterStrikeSharp installation.

using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using UnbanMenuPlugin;

namespace UnbanMenuPlugin
{
    // Replace/adjust base class or attributes depending on your CSSharp plugin API.
    public class UnbanMenuPlugin /* : PluginBase or similar */
    {
        // Called on plugin load by CSSharp (method name may differ)
        public void OnLoad()
        {
            // Ensure config exists and is loaded
            Config.LoadOrCreate();

            // Register client command (adjust to your Commands API)
            try
            {
                // Example placeholder: Commands.RegisterClientCommand(Config.Data.Command, OnOpenUnbanMenu);
                Console.WriteLine($"[UnbanMenu] Registered command: {Config.Data.Command}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[UnbanMenu] Error registering command: " + ex.Message);
            }
        }

        // Example client command handler (signature will vary)
        public void OnOpenUnbanMenu(object clientObj, string[] args)
        {
            // Convert/validate client object to your Client type
            dynamic client = clientObj;
            if (client == null) return;

            // Permission check
            if (Config.Data.RequireRootFlag)
            {
                try
                {
                    // Adjust to your client API, e.g., client.HasFlag(Config.Data.RootFlag) or client.Flags.Contains(...)
                    bool hasRoot = false;
                    try { hasRoot = client.HasFlag(Config.Data.RootFlag); } catch { }
                    try { hasRoot = hasRoot || (client.Flags != null && client.Flags.Contains(Config.Data.RootFlag)); } catch { }
                    if (!hasRoot)
                    {
                        // Send message to client in-game (adjust to your API)
                        try { client.PrintToChat("Du brauchst das @css/root Flag."); } catch { Console.WriteLine("[UnbanMenu] Permission denied for client."); }
                        return;
                    }
                }
                catch { /* ignore permission check errors, but block access by default */ }
            }

            var bans = GetActiveBansFromDb();
            if (bans.Count == 0)
            {
                try { client.PrintToChat("Keine aktiven Bans gefunden."); } catch { Console.WriteLine("[UnbanMenu] No bans."); }
                return;
            }

            // Build menu using your Menu API. Below is a conceptual example — replace with your actual menu creation calls.
            try
            {
                // Example pseudo-code:
                // var menu = MenuFactory.CreateMenu(Config.Data.MenuTitle, client);
                // foreach(var b in bans) menu.AddItem($"{b.Id}: {b.SteamId} - {b.Reason}", $"unban_do {b.Id}");
                // menu.Send();

                // If your API can't accept commands as strings, register a selection callback instead.
                Console.WriteLine("[UnbanMenu] Would open menu with " + bans.Count + " entries.");
                // For quick fallback: print list to chat with indices
                for (int i = 0; i < bans.Count; i++)
                {
                    try { client.PrintToChat($"{i+1}. {bans[i].SteamId} - {bans[i].Reason}"); } catch { }
                }
                try { client.PrintToChat("Benutze /unban <Nummer> im Chat (oder implementiere menu callback)."); } catch { }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[UnbanMenu] Failed to build/show menu: " + ex.Message);
            }
        }

        // Optional: implement a chat/console command to unban by index
        public void OnUnbanByIndex(object clientObj, string[] args)
        {
            dynamic client = clientObj;
            if (args == null || args.Length == 0) { try { client.PrintToChat("Usage: /unban <Nummer>"); } catch { } return; }
            if (!int.TryParse(args[0], out int idx)) { try { client.PrintToChat("Ungültige Nummer"); } catch { } return; }

            var bans = GetActiveBansFromDb();
            if (idx < 1 || idx > bans.Count) { try { client.PrintToChat("Nummer außerhalb Bereich"); } catch { } return; }

            var record = bans[idx - 1];
            if (RemoveBanById(record.Id))
            {
                try { client.PrintToChat($"Ban für {record.SteamId} entfernt."); } catch { }
                if (Config.Data.LogUnbans) Console.WriteLine($"[UnbanMenu] {client} removed ban id={record.Id} steam={record.SteamId}");
            }
            else
            {
                try { client.PrintToChat("Fehler beim Entfernen des Bans."); } catch { }
            }
        }

        // DB helper: read active bans
        private List<BanRecord> GetActiveBansFromDb()
        {
            var list = new List<BanRecord>();
            var cfg = Config.Data;
            string connStr = $"Server={cfg.DatabaseHost};Port={cfg.DatabasePort};Database={cfg.DatabaseName};Uid={cfg.DatabaseUser};Pwd={cfg.DatabasePassword};SslMode=Preferred;";

            try
            {
                using (var conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string sql = $"SELECT id, steamid, reason FROM `{cfg.BansTable}` WHERE active = 1";
                    using (var cmd = new MySqlCommand(sql, conn))
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            int id = r.GetInt32(0);
                            string steam = r.IsDBNull(1) ? "" : r.GetString(1);
                            string reason = r.FieldCount > 2 && !r.IsDBNull(2) ? r.GetString(2) : "No reason";
                            list.Add(new BanRecord { Id = id, SteamId = steam, Reason = reason });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[UnbanMenu] DB error: " + ex.Message);
            }

            return list;
        }

        private bool RemoveBanById(int id)
        {
            var cfg = Config.Data;
            string connStr = $"Server={cfg.DatabaseHost};Port={cfg.DatabasePort};Database={cfg.DatabaseName};Uid={cfg.DatabaseUser};Pwd={cfg.DatabasePassword};SslMode=Preferred;";

            try
            {
                using (var conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string sql = $"UPDATE `{cfg.BansTable}` SET active = 0 WHERE id = @id";
                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        int affected = cmd.ExecuteNonQuery();
                        return affected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[UnbanMenu] DB error on remove: " + ex.Message);
                return false;
            }
        }
    }

    public class BanRecord { public int Id; public string SteamId; public string Reason; }
}
