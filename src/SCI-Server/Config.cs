using SCI_Logger;
using System.Text.Json;

namespace SCI_Server
{
	public class Config
	{
		/// <summary>
		/// Global JsonOptions
		/// </summary>
		public static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

		public static readonly string DIR_ROOT = "server/";
		public static readonly string DIR_DATABASE = DIR_ROOT + "data/";
		public static readonly string CONFIG_FILE = DIR_ROOT + "config.json";

		public static readonly string CRYPT_PASSWORD = "Test123";
		public static readonly string DATAFILE_EXTENTION = ".dat";

		public static ConfigObject? currentConfig;

		/// <summary>
		/// Load config from file
		/// </summary>
		public static void Init()
		{
			if (!File.Exists(CONFIG_FILE))
			{
				Logging.Log(Logging.LogLevel.WARN, "No config file found. Create default config");
				try
				{
					File.WriteAllText(CONFIG_FILE, JsonSerializer.Serialize(new ConfigObject(), JsonOptions));
				}
				catch (UnauthorizedAccessException)
				{
					Logging.Log(Logging.LogLevel.ERROR, "No permission to create files in this directory");
					Environment.Exit(1);
				}
			}
			Logging.Log(Logging.LogLevel.INFO, "Init Config");
			currentConfig = JsonSerializer.Deserialize<ConfigObject>(File.ReadAllText(CONFIG_FILE), JsonOptions);
		}

		/// <summary>
		/// Save current config
		/// </summary>
		public static void SaveConfig()
		{
			try
			{
				File.WriteAllText(CONFIG_FILE, JsonSerializer.Serialize(currentConfig, JsonOptions));
				Logging.Log(Logging.LogLevel.INFO, "Config saved");
			}
			catch (UnauthorizedAccessException)
			{
				Logging.Log(Logging.LogLevel.ERROR, "No permission to create files in this directory");
				Environment.Exit(1);
			}
		}

		/// <summary>
		/// Primary Config Interface
		/// </summary>
		public class ConfigObject
		{
			public string ServerAddress { get; set; } = "127.0.0.1";
			public int ServerPort { get; set; } = 8080;
			public string SerialComModulePortName { get; set; } = "COM1";
			public int SerialComBaudrate { get; set; } = 9600;
		}
	}
}
