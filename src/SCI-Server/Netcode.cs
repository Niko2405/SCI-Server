using SCI_Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCI_Server
{
	internal class Netcode
	{
		public static List<string> databaseCommands = [];
		public static List<string> systemCommands = [];

		public Netcode()
		{
			Logging.PrintHeader("Register NETCODE");
			systemCommands.Add("[system] help");
			systemCommands.Add("[system] ping");

			databaseCommands.Add("[database] create user");
			databaseCommands.Add("[database] delete user");

			// [database] login $username $password $accesscode $secruitykey
			databaseCommands.Add("[database] login");

			databaseCommands.Add("[database] update user username");
			databaseCommands.Add("[database] update user password");
			databaseCommands.Add("[database] update user permission");
			databaseCommands.Add("[database] update user birthday");
			databaseCommands.Add("[database] update user country");
			databaseCommands.Add("[database] update user state");
			databaseCommands.Add("[database] update user age");
			databaseCommands.Add("[database] update user locked");

			databaseCommands.Add("[database] get user");
			databaseCommands.Add("[database] get users");

			Logging.Info($"databaseCommands: {databaseCommands.Count} commands registered");
			Logging.Info($"systemCommands: {systemCommands.Count} commands registered");
		}

		public static string ProcessCommand(string command)
		{
			string response = string.Empty;
			string[] rawCmd = command.Split([' ']);
			Logging.Info($"Received command: {command}");
			try
			{
				if (command.Contains("[system]"))
				{
					for (int a = 0; a < rawCmd.Length; a++)
					{
						if (rawCmd[a] == "help")
						{
							response = "No help. Sorry";
						}
					}
					return response;
				}
				else if (command.Contains("[database]"))
				{
					for (int a = 0; a < rawCmd.Length; a++)
					{
						if (rawCmd[a] == "login")
						{
							if (rawCmd.Length == 6)
							{
								// [database] login $username $password $accesscode $secruitykey
								if (DatabaseManager.IsLoginValid(rawCmd[2], rawCmd[3], Convert.ToInt16(rawCmd[4]), Convert.ToInt16(rawCmd[5])))
								{
									return "LOGIN: OK";
								}
								else
								{
									return "LOGIN: FAILED";
								}
							}
							else
							{
								return "UserInput ERROR";
							}

						}
					}
					return "No command found";
				}
				else
				{
					return "ERR";
				}
			}
			catch (Exception ex)
			{
				Logging.Error(ex.Message);
				return ex.Message;
			}
		}
	}
}
