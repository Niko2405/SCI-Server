using SCI_Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCI_Server
{
	internal class Init
	{
		public static void CreateFilesystem()
		{
			try
			{
				Directory.CreateDirectory(Data.DIR_ROOT);
				Directory.CreateDirectory(Data.DIR_DATABASE);
				Directory.CreateDirectory(Data.DIR_DATABASE);
			}
			catch (Exception ex)
			{
				Logging.Log(Logging.LogLevel.ERROR, ex.Message);
			}
		}
		public static void DeleteFilesystem() { }

		/// <summary>
		/// Create and init database
		/// </summary>
		public static void CheckDatabase()
		{
			if (DatabaseManager.CreateUserTable())
				Logging.Log(Logging.LogLevel.INFO, "OK");
			// create default root
			if (DatabaseManager.AddUser("root", "Admin123", "NULL", "NULL", "root", 0))
				Logging.Log(Logging.LogLevel.INFO, "OK");
		}
	}
}
