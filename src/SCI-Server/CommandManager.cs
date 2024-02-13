using SCI_Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCI_Server
{
	internal class CommandManager
	{
		public List<string> SystemCommands = [
			"help",
			"info"
		];
		public List<string> DatabaseCommands = [
			"db add user",
			"db remove user",

			"db set user username",
			"db set user password",
			"db set user firstname",
			"db set user lastname",
			"db set user permission",
			"db set user locked",

			"db get user username",
			"db get user password",
			"db get user firstname",
			"db get user lastname",
			"db get user permission",
			"db get user locked",

			"db get user data",
			"db get user allUsernames",
			"db login user",
		];
		public CommandManager()
		{
			Logging.Log(Logging.LogLevel.DEBUG, $"SystemCommands:\t\t{SystemCommands.Count} commands available");
			Logging.Log(Logging.LogLevel.DEBUG, $"DatabaseCommands:\t{DatabaseCommands.Count} commands available");
		}

		/// <summary>
		/// Process the given command and create a result of them
		/// </summary>
		/// <param name="command"></param>
		/// <returns>String as result, LogLevel of this operation</returns>
		public string ProcessCommand(string command)
		{
			string[] rawCommand = command.Split(' ');
			try
			{
				#region HELP
				if (command.Contains(SystemCommands[0]))
				{
					return "Sorry, no help";
				}
				#endregion
				else if (command.Contains(SystemCommands[1]))
				{
					return "No info";
				}
				#region DB_ADD_USER
				else if (command.Contains(DatabaseCommands[0]))
				{
					if (DataManager.AddUserProfile(rawCommand[3], rawCommand[4], rawCommand[5], rawCommand[6], rawCommand[7], Convert.ToBoolean(rawCommand[8])))
						return "DATABASE: OK";
					else
						return "DATABASE: FAILED";
				}
				#endregion
				#region DB_REMOVE_USER
				else if (command.Contains(DatabaseCommands[1]))
				{
					if (DataManager.RemoveUserProfile(rawCommand[3]))
						return "DATABASE: OK";
					else
						return "DATABASE: FAILED";
				}
				#endregion
				else
				{
					return "Command not found";
				}
			}
			catch (IndexOutOfRangeException ex)
			{
				Logging.Log(Logging.LogLevel.ERROR, ex.Message);
				return ex.Message;
			}
		}
	}
}
