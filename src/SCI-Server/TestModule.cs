using SCI_Logger;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SCI_Server
{
	internal class TestModule
	{
		public required int ID { get; set; }
		public required string Name { get; set; }
		public required string Description { get; set; }

		public static void StartTest()
		{
			#region TEST LOGGER
			Logging.PrintHeader("Test Logger System");
			Thread.Sleep(2500);
			Logging.Log(Logging.LogLevel.INFO, "Manual Log detecter...");

			Logging.Log(Logging.LogLevel.INFO, "This is an info message");
			Logging.Log(Logging.LogLevel.WARN, "This is an warn message");
			Logging.Log(Logging.LogLevel.ERROR, "This is an error message");
			Logging.Log(Logging.LogLevel.DEBUG, "This is an debug message");
			#endregion

			#region New Data Manager
			Logging.PrintHeader("Test JSON Data Manager");
			Logging.PrintHeader("Add user test");
			Logging.Log(Logging.LogLevel.INFO, "Add user1");
			DataManager.AddUserProfile("user1", "password", "admin", "user1", "ABC", false);
			Thread.Sleep(1500);

			Logging.Log(Logging.LogLevel.INFO, "Add user2");
			DataManager.AddUserProfile("user2", "password", "admin", "user2", "ABC", false);
			Thread.Sleep(1500);

			Logging.Log(Logging.LogLevel.INFO, "Add user3");
			DataManager.AddUserProfile("user3", "password", "admin", "user3", "ABC", false);
			Thread.Sleep(1500);

			Logging.PrintHeader("Remove user test");
			Logging.Log(Logging.LogLevel.INFO, "Remove user3");
			DataManager.RemoveUserProfile("user3");
			Thread.Sleep(1500);

			Logging.PrintHeader("Change username test");
			Logging.Log(Logging.LogLevel.INFO, "Change username of user2");
			DataManager.ChangeUsername("user2", "user4");
			Thread.Sleep(1500);

			Logging.PrintHeader("Change password test");
			Logging.Log(Logging.LogLevel.INFO, "Change Password of user4");
			DataManager.ChangePassword("user4", "NeuesPasswort");
			Thread.Sleep(1500);

			Logging.PrintHeader("Change permission test");
			Logging.Log(Logging.LogLevel.INFO, "Change Permission of user1");
			DataManager.ChangePermission("user1", "testPermission");
			Thread.Sleep(1500);

			Logging.PrintHeader("Check login test");
			Logging.Log(Logging.LogLevel.INFO, "Check login for user4");
			DataManager.IsLoginValid("user4", "NeuesPasswort");
			Thread.Sleep(2500);

			Logging.PrintHeader("Remove test user profiles");
			Logging.Log(Logging.LogLevel.INFO, "Delete test user profiles");
			DataManager.RemoveUserProfile("user1");
			DataManager.RemoveUserProfile("user4");
			#endregion

			#region Multiple JSON SourceGenerator test
			Logging.PrintHeader("Create new test JSON file");
			var TestData = new TestModule()
			{
				ID = 1,
				Description = "Test",
				Name = "TestName",
			};
			string jsonString = JsonSerializer.Serialize(TestData, Config.JsonOptions);

			// write new data
			File.WriteAllText(Config.DIR_DATA + "TestAOT.json", jsonString);
			#endregion

			#region Test Crypt
			Logging.PrintHeader("Crypt Test");

			var rand = new Random();
			int count = 500;
			long[] buffer = new long[count];

			Logging.Log(Logging.LogLevel.INFO, $"Start filling the buffer: {count}");
			for (int i = 0; i < count; i++)
			{
				buffer[i] = rand.NextInt64(long.MaxValue / 2, long.MaxValue);
			}
			Logging.Log(Logging.LogLevel.INFO, "Filling buffer finish");
			Logging.Log(Logging.LogLevel.INFO, "Start encryp...");
			for (int i = 0; i < buffer.Length; i++)
			{
				string xCrypted = Crypt.Encrypt(buffer[i].ToString());
				string xEncrypted = Crypt.Decrypt(xCrypted);
				Logging.Log(Logging.LogLevel.INFO, $"Run Test [{i}/{count}]\tOriginal Value: {buffer[i]}\tSHA512 Value: {xCrypted}\tEncrypted Value: {xEncrypted}", false);

				if (buffer[i].ToString() != xEncrypted)
					Logging.Log(Logging.LogLevel.ERROR, $"Crypt Overflow: Len Original Value:{buffer[i].ToString().Length}\tLen Encrypted Value: {xEncrypted.Length}");
			}
			#endregion

			#region RS232 Test
			Logging.PrintHeader("RS232 Test");
			Logging.Log(Logging.LogLevel.INFO, "Send 'Hello World'");
			//SerialComModule.RS232.SendCommand("Hello World");
			#endregion
		}
	}
}
