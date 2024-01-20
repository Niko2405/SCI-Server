using SCI_Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SCI_Server
{
	internal class CommandManager
	{
		public List<string> SystemCommands = [];
		public List<string> DatabaseCommands = [];
		public CommandManager()
		{
			SystemCommands.Add("system help");

			DatabaseCommands.Add("database add user");
			DatabaseCommands.Add("database remove user");

			// set
			DatabaseCommands.Add("database set user username");
			DatabaseCommands.Add("database set user password");
			DatabaseCommands.Add("database set user firstname");
			DatabaseCommands.Add("database set user lastname");
			DatabaseCommands.Add("database set user permission");
			DatabaseCommands.Add("database set user lockedState");

			// get
			DatabaseCommands.Add("database get user username");
			DatabaseCommands.Add("database get user password");
			DatabaseCommands.Add("database get user firstname");
			DatabaseCommands.Add("database get user lastname");
			DatabaseCommands.Add("database get user permission");
			DatabaseCommands.Add("database get user lockedState");

			DatabaseCommands.Add("database get allUsers");
			DatabaseCommands.Add("database get userData");

			// login
			DatabaseCommands.Add("database login user");

			Logging.Log(Logging.LogLevel.DEBUG, $"SystemCommands:\t{SystemCommands.Count} commands available");
			Logging.Log(Logging.LogLevel.DEBUG, $"DatabaseCommands:\t{DatabaseCommands.Count} commands available");
		}

		/// <summary>
		/// Process the given command and create a result of them
		/// </summary>
		/// <param name="command"></param>
		/// <returns>String as result, LogLevel of this operation</returns>
		public Tuple<string, Logging.LogLevel> ProcessCommand(string command)
		{
			string[] rawCommand = command.Split(' ');
			try
			{
				// system help
				if (command.Contains(SystemCommands[0]))
				{
					string result = "\n\t\t===> SystemCommands <===\n";
					for (int i = 0; i < SystemCommands.Count; i++)
					{
						result += SystemCommands[i] + "\n";
					}
					result += "\n\t\t===> DatabaseCommands <===\n";
					for (int i = 0; i < DatabaseCommands.Count; i++)
					{
						result += DatabaseCommands[i] + "\n";
					}
					return Tuple.Create(result, Logging.LogLevel.INFO);
				}

				// database add user $username $password $firstname $lastname $permission $locked
				if (command.StartsWith(DatabaseCommands[0]))
				{
					if (DatabaseManager.AddUser(rawCommand[3], rawCommand[4], rawCommand[5], rawCommand[6], rawCommand[7], Convert.ToInt32(rawCommand[8])))
					{
						return Tuple.Create("DATABASE: OK", Logging.LogLevel.INFO);
					}
					else
					{
						return Tuple.Create("DATABASE: FAILED", Logging.LogLevel.ERROR);
					}
				}
				// database remove user $username
				if (command.StartsWith(DatabaseCommands[1]))
				{
					if (DatabaseManager.RemoveUser(rawCommand[3]))
					{
						return Tuple.Create("DATABASE: OK", Logging.LogLevel.INFO);
					}
					else
					{
						return Tuple.Create("DATABASE: FAILED", Logging.LogLevel.ERROR);
					}
				}
				// database set user username $username $newUsername
				if (command.StartsWith(DatabaseCommands[2]))
				{
					if (DatabaseManager.SetUsername(rawCommand[4], rawCommand[5]))
					{
						return Tuple.Create("DATABASE: OK", Logging.LogLevel.INFO);
					}
					else
					{
						return Tuple.Create("DATABASE: FAILED", Logging.LogLevel.ERROR);
					}
				}
				// database set user firstname $username $newFirstname
				if (command.StartsWith(DatabaseCommands[4]))
				{
					if (DatabaseManager.SetFirstname(rawCommand[4], rawCommand[5]))
					{
						return Tuple.Create("DATABASE: OK", Logging.LogLevel.INFO);
					}
					else
					{
						return Tuple.Create("DATABASE: FAILED", Logging.LogLevel.ERROR);
					}
				}
			}
			catch (Exception ex)
			{
				return Tuple.Create(ex.Message, Logging.LogLevel.ERROR);
			}
			return Tuple.Create("No command found", Logging.LogLevel.ERROR);
		}
	}
}
