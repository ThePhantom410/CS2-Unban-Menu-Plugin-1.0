using System;
using System.IO;
using System.Text.Json;

namespace UnbanMenuPlugin
{
    public class ConfigModel
    {
        public string DatabaseHost { get; set; } = "127.0.0.1";
        public int DatabasePort { get; set; } = 3306;
        public string DatabaseUser { get; set; } = "root";
        public string DatabasePassword { get; set; } = "password";
        public string DatabaseName { get; set; } = "cs2_admin";
        public string BansTable { get; set; } = "cs_admin_bans";
        public string MenuTitle { get; set; } = "Unban Player Menu";
        public string Command { get; set; } = "openunbanmenu";
        public string RootFlag { get; set; } = "@css/root";
        public bool RequireRootFlag { get; set; } = true;
        public bool LogUnbans { get; set; } = true;
    }

    public static class Config
    {
        private static readonly string ConfigPath = Path.Combine("addons", "counterstrikesharp", "configs", "unbanmenu", "config.json");
        public static ConfigModel Data { get; private set; } = new ConfigModel();

        public static void LoadOrCreate()
        {
            try
            {
                var dir = Path.GetDirectoryName(ConfigPath);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                if (!File.Exists(ConfigPath))
                {
                    var json = JsonSerializer.Serialize(Data, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(ConfigPath, json);
                    Console.WriteLine("[UnbanMenu] Default config.json created at: " + ConfigPath);
                }
                else
                {
                    var json = File.ReadAllText(ConfigPath);
                    Data = JsonSerializer.Deserialize<ConfigModel>(json) ?? new ConfigModel();
                    Console.WriteLine("[UnbanMenu] Loaded config.json from: " + ConfigPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[UnbanMenu] Failed to load/create config: " + ex.Message);
                Data = new ConfigModel();
            }
        }
    }
}
