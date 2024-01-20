using SCI_Logger;
using System.Windows.Input;

namespace SCI_Server
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine($"Logger Version: {Logging.VERSION}");
			Logging.IsDebugEnabled = true;

			foreach (string arg in args)
			{
				if (arg == "--test")
				{
					Environment.Exit(0);
				}
			}

			// start init
			Init.CreateFilesystem();
			Init.CheckDatabase();

			ServerSocket server = new("0.0.0.0", 8080);
			Thread serverThread = new(server.StartListener);
			serverThread.Start();

			CommandManager manager = new();
			Thread.Sleep(1000);
			while (true)
			{
				Console.Write(">");
				string cmd = Console.ReadLine();
				Tuple<string, Logging.LogLevel> result = manager.ProcessCommand(cmd);
				Logging.Log(result.Item2, result.Item1);
			}
		}
	}
}
