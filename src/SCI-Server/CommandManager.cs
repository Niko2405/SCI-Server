using System;
using SCI_Logger;

namespace SCI_Server
{
	internal class CommandManager
	{
		public readonly List<string> CommandList = [
			"help",
			"info",
			"encrypt",
			"decrypt",

			"db add user",
			"db remove user",

			"db set user username",
			"db set user password",
			"db set user permission",
			"db set user firstname",
			"db set user lastname",
			"db set user locked",

			"db get user username",
			"db get user password",
			"db get user permission",
			"db get user firstname",
			"db get user lastname",
			"db get user locked",

			"db getAll users",
			"db getAll userData",
			"db login user",

			"serial send",
			"serial test",
			"serial get config"
		];

		public CommandManager()
		{
			Logging.Log(Logging.LogLevel.DEBUG, $"There are: {CommandList.Count} commands registered");
		}

		/// <summary>
		/// Process the given command and create a result of them
		/// </summary>
		/// <param name="command"></param>
		/// <returns>String as result, LogLevel of this operation</returns>
		public string ProcessCommand(string InputCommand)
		{
			string[] rawCommand = InputCommand.Split(' ');
			try
			{
				foreach (string command in CommandList)
				{
					// When a string match, use it
					if (command == InputCommand)
					{
						#region SYSTEM
						// help
						if (command == CommandList[0])
						{
							return "Help Page";
						}
						// info
						else if (command == CommandList[1])
						{
							return "Info Page";
						}
						// encrypt
						else if (command == CommandList[2])
						{
							return "Need args";
						}
						// decrypt
						else if (command == CommandList[3])
						{
							return "Need args";
						}
						#endregion

						// db add user
						else if (command == CommandList[4])
						{
							return "Need args";
						}
						// db remove user
						else if (command == CommandList[5])
						{
							return "Need args";
						}

						#region SET
						// db set user username
						else if (command == CommandList[6])
						{
							return "Need args";
						}
						// db set user password
						else if (command == CommandList[7])
						{
							return "Need args";
						}
						// db set user permission
						else if (command == CommandList[8])
						{
							return "Need args";
						}
						// db set user firstname
						else if (command == CommandList[9])
						{
							return "Need args";
						}
						// db set user lastname
						else if (command == CommandList[10])
						{
							return "Need args";
						}
						// db set user locked
						else if (command == CommandList[11])
						{
							return "Need args";
						}
						#endregion

						#region GET
						// db get user username
						else if (command == CommandList[12])
						{
							return "Need args";
						}
						// db get user password
						else if (command == CommandList[13])
						{
							return "Need args";
						}
						// db get user permission
						else if (command == CommandList[14])
						{
							return "Need args";
						}
						// db get user firstname
						else if (command == CommandList[15])
						{
							return "Need args";
						}
						// db get user lastname
						else if (command == CommandList[16])
						{
							return "Need args";
						}
						// db get user locked
						else if (command == CommandList[17])
						{
							return "Need args";
						}
						#endregion

						// db getAll users
						else if (command == CommandList[18])
						{
							return "Need args";
						}
						// db getAll userData
						else if (command == CommandList[19])
						{
							return "Need args";
						}
						// db login user
						else if (command == CommandList[20])
						{
							return "Need args";
						}

						#region SERIAL
						// serial send
						else if (command == CommandList[21])
						{
							return "Need args";
						}
						// serial test
						else if (command == CommandList[22])
						{
							return "Need args";
						}
						// serial get config
						else if (command == CommandList[23])
						{
							return "Need args";
						}
						#endregion
					}
				}
				return "No command found";
			}
			catch (IndexOutOfRangeException ex)
			{
				Logging.Log(Logging.LogLevel.ERROR, ex.Message);
				return ex.Message;
			}
		}
	}
}
