using SCI_Logger;

namespace SCI_Server
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine($"Logger Version:\t\t{Logging.VERSION}");
			Console.WriteLine($"Database Version:\t{DatabaseManager.VERSION}");
			
			// start init
			Init.CheckFilesystem();
			Config.Init();

			foreach (string arg in args)
			{
				if (arg == "--debug")
				{
					Logging.DebugEnabled = true;
					Logging.Log(Logging.LogLevel.DEBUG, "Debug is enabled");
				}
				if (arg == "--test")
				{
					Console.Title = "SCI-Server TestModule";
					TestModule.StartTest();
					Environment.Exit(0);
				}
			}

			Console.Title = "SCI-Server";
			if (Config.currentConfig != null)
			{
				ServerSocket server = new(Config.currentConfig.ServerAddress, Config.currentConfig.ServerPort);
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
						if (cmd == "exit")
						{
							Config.SaveConfig();
							Environment.Exit(0);
						}
						Console.WriteLine(manager.ProcessCommand(cmd));
					}

				}
			}
			else
			{
				Logging.Log(Logging.LogLevel.ERROR, "Config cannot load");
			}
		}
	}
}
