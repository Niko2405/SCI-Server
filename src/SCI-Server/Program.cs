using SCI_Logger;
using System.Text.Json;

namespace SCI_Server
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine($"Logger Version: {Logging.VERSION}");
			Logging.SelectDebugMode(true);

			// start init
			Init.CreateFilesystem();
			Init.CheckDatabase();

			foreach (string arg in args)
			{
				if (arg == "--test")
				{
					TestModule.StartTest();
					Environment.Exit(0);
				}
			}

			ServerSocket server = new("0.0.0.0", 8080);
			Thread serverThread = new(server.StartListener);
			serverThread.Start();

			CommandManager manager = new();
			Thread.Sleep(1000);
			while (true)
			{
				Console.Write(">");
				string? cmd = Console.ReadLine();
				if (cmd != null)
				{
					manager.ProcessCommand(cmd);
				}
			}
		}
	}
}
