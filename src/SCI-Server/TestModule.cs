﻿using SCI_Logger;
using System.Text.Json;

namespace SCI_Server
{
	internal class TestModule
	{
		public required int ID { get; set; }
		public required string Name { get; set; }
		public required string Description { get; set; }

		private static readonly int DELAY = 100;
		/// <summary>
		/// Start primary test
		/// </summary>
		public static void StartTest()
		{
			RunTestLoggerSystem();
			RunTestDataManager();
			RunTestMultipleJsonContext();
			RunTestCrypt();
			RunTestRS232();
		}

		private static void RunTestLoggerSystem()
		{
			Logging.PrintHeader("RunTestLoggerSystem");
			Thread.Sleep(DELAY);
			Logging.Log(Logging.LogLevel.INFO, "Manual Log detecter...");

			Logging.Log(Logging.LogLevel.INFO, "This is an info message");
			Logging.Log(Logging.LogLevel.WARN, "This is an warn message");
			Logging.Log(Logging.LogLevel.ERROR, "This is an error message");
			Logging.Log(Logging.LogLevel.DEBUG, "This is an debug message");
		}

		private static void RunTestDataManager()
		{
			Logging.PrintHeader("RunTestDataManager");
			Logging.PrintHeader("Add user test");
			Logging.Log(Logging.LogLevel.INFO, "Add user1");
			DataManager.AddUserProfile("user1", "password", "admin", "user1", "ABC", false);
			Thread.Sleep(DELAY);

			Logging.Log(Logging.LogLevel.INFO, "Add user2");
			DataManager.AddUserProfile("user2", "password", "admin", "user2", "ABC", false);
			Thread.Sleep(DELAY);

			Logging.Log(Logging.LogLevel.INFO, "Add user3");
			DataManager.AddUserProfile("user3", "password", "admin", "user3", "ABC", false);
			Thread.Sleep(DELAY);

			Logging.PrintHeader("Remove user test");
			Logging.Log(Logging.LogLevel.INFO, "Remove user3");
			DataManager.RemoveUserProfile("user3");
			Thread.Sleep(DELAY);

			Logging.PrintHeader("Change username test");
			Logging.Log(Logging.LogLevel.INFO, "Change username of user2");
			DataManager.ChangeUsername("user2", "user4");
			Thread.Sleep(DELAY);

			Logging.PrintHeader("Change password test");
			Logging.Log(Logging.LogLevel.INFO, "Change Password of user4");
			DataManager.ChangePassword("user4", "NeuesPasswort");
			Thread.Sleep(DELAY);

			Logging.PrintHeader("Change permission test");
			Logging.Log(Logging.LogLevel.INFO, "Change Permission of user1");
			DataManager.ChangePermission("user1", "testPermission");
			Thread.Sleep(DELAY);

			Logging.PrintHeader("Check login test");
			Logging.Log(Logging.LogLevel.INFO, "Check login for user4");
			DataManager.IsLoginValid("user4", "NeuesPasswort");
			Thread.Sleep(DELAY);

			Logging.PrintHeader("Remove test user profiles");
			Logging.Log(Logging.LogLevel.INFO, "Delete test user profiles");
			DataManager.RemoveUserProfile("user1");
			DataManager.RemoveUserProfile("user4");
		}

		private static void RunTestMultipleJsonContext()
		{
			Logging.PrintHeader("RunTestMultipleJsonContext");
			var TestData = new TestModule()
			{
				ID = 1,
				Description = "Test",
				Name = "TestName",
			};
			string jsonString = JsonSerializer.Serialize(TestData, Config.JsonOptions);

			// write new data
			File.WriteAllText(Config.DIR_DATA + "TestAOT.json", jsonString);
		}

		private static void RunTestCrypt()
		{
			Logging.PrintHeader("RunTestCrypt");

			var rand = new Random();
			int count = 100;
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
				Logging.Log(Logging.LogLevel.INFO, $"Run Test [{i + 1}/{count}]\tOriginal Value: {buffer[i]}\tSHA512 Value: {xCrypted}\tEncrypted Value: {xEncrypted}", false);

				if (buffer[i].ToString() != xEncrypted)
					Logging.Log(Logging.LogLevel.ERROR, $"Crypt Overflow: Len Original Value:{buffer[i].ToString().Length}\tLen Encrypted Value: {xEncrypted.Length}");
			}
		}

		private static void RunTestRS232()
		{
			Logging.PrintHeader("RunTestRS232");
			Logging.Log(Logging.LogLevel.INFO, "Send command: Ping");
			string response = SerialComModule.RS232.SendCommand("Ping", true);
			if (response == "Pong")
			{
				Logging.Log(Logging.LogLevel.INFO, $"Response: {response}. Test OK");
			}
			else if (response != "Pong")
			{
				Logging.Log(Logging.LogLevel.ERROR, "No response");
			}
		}
	}
}
