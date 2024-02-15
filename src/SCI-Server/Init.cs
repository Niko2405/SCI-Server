using SCI_Logger;
namespace SCI_Server
{
	internal class Init
	{
		public static void CheckFilesystem()
		{
			try
			{
				Directory.CreateDirectory(Config.DIR_ROOT);
				Directory.CreateDirectory(Config.DIR_DATA);
			}
			catch (Exception ex)
			{
				Logging.Log(Logging.LogLevel.ERROR, ex.Message);
			}
		}
		public static void CheckConfig()
		{
			if (!File.Exists(Config.CONFIG_FILE))
			{
				Logging.Log(Logging.LogLevel.WARN, "No config.json found. Create default config file");
				//Configs.CreateDefaultConfig();
			}
		}
		public static void DeleteFilesystem() { }

		/// <summary>
		/// Create and init database
		/// </summary>
		public static void CheckDatabase()
		{

		}
	}
}
