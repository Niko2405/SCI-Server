using SCI_Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCI_Server
{
	internal class TestModule
	{
		public static void StartTest()
		{
			#region TEST LOGGER
			Logging.PrintHeader("Test Logger System");
			Logging.Log(Logging.LogLevel.INFO, "Manual Log detecter...");

			Logging.Log(Logging.LogLevel.INFO, "This is an info message");
			Logging.Log(Logging.LogLevel.WARN, "This is an warn message");
			Logging.Log(Logging.LogLevel.ERROR, "This is an error message");
			Logging.Log(Logging.LogLevel.DEBUG, "This is an debug message");
			#endregion

			#region New Data Manager
			Logging.PrintHeader("Test JSON Data Manager");
			Logging.Log(Logging.LogLevel.INFO, "Add user1");
			DataManager.AddUserProfile("user1", "password", "admin", "user1", "ABC", false);
			Thread.Sleep(1500);

			Logging.Log(Logging.LogLevel.INFO, "Add user2");
			DataManager.AddUserProfile("user2", "password", "admin", "user2", "ABC", false);
			Thread.Sleep(1500);

			Logging.Log(Logging.LogLevel.INFO, "Add user3");
			DataManager.AddUserProfile("user3", "password", "admin", "user3", "ABC", false);
			Thread.Sleep(1500);

			Logging.Log(Logging.LogLevel.INFO, "Remove user3");
			DataManager.RemoveUserProfile("user3");
			Thread.Sleep(1500);

			Logging.Log(Logging.LogLevel.INFO, "Change username of user2");
			DataManager.ChangeUsername("user2", "user4");
			Thread.Sleep(1500);

			Logging.Log(Logging.LogLevel.INFO, "Change Password of user4");
			DataManager.ChangePassword("user4", "NeuesPasswort");
			Thread.Sleep(1500);

			Logging.Log(Logging.LogLevel.INFO, "Change Permission of user1");
			DataManager.ChangePermission("user1", "testPermission");
			Thread.Sleep(1500);

			Logging.Log(Logging.LogLevel.INFO, "Check login for user4");
			DataManager.IsLoginValid("user4", "NeuesPasswort");
			Thread.Sleep(2500);

			Logging.Log(Logging.LogLevel.INFO, "Delete test user profiles");
			DataManager.RemoveUserProfile("user1");
			DataManager.RemoveUserProfile("user4");
			#endregion
		}
	}
}
