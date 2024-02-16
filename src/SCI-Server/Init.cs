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
				Directory.CreateDirectory(Config.DIR_DATABASE);
			}
			catch (Exception ex)
			{
				Logging.Log(Logging.LogLevel.ERROR, ex.Message);
			}
		}
	}
}
