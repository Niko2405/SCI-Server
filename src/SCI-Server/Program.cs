﻿using SCI_Logger;
using System.Text.Json;

namespace SCI_Server
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine($"Logger Version: {Logging.VERSION}");
			Logging.DebugEnabled = true;

			// start init
			Init.CheckFilesystem();
			Init.CheckConfig();
			Init.CheckDatabase();
			Config.Init();

			foreach (string arg in args)
			{
				if (arg == "--test")
				{
					TestModule.StartTest();
					Environment.Exit(0);
				}
			}

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
						manager.ProcessCommand(cmd);
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
