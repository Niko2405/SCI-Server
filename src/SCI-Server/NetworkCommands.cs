using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SCI_Logger;

namespace SCI_Server
{
	internal class NetworkCommands
	{
		public static string[] commandList = {
			"help",
			"ping",

			"database update profile password",
			"database update profile username",
			"database update profile permission",
			"database update profile locked",
			"database update profile birthday",
			"database update profile country",
			"database update profile state",
			"database update profile age",

			"database create profile",
			"database delete profile",
			"database login profile",

			 "database get profile",

		};
		public static string ProcessCommand(string rawCommand)
		{
			Logging.Debug($"There are {commandList.Length} commands registered");
			try
			{
				Logging.Debug($"Command Length: {rawCommand.Length}");
				for (int index = 0; index < commandList.Length; index++)
				{
					if (rawCommand.Contains(commandList[index]))
					{
						#region help
						if (commandList[index].Equals("help"))
						{
							string response = " ---> HELP PAGE <--\n";
							for (int i = 0; i < commandList.Length; i++)
							{
								response += commandList[i] + ":\n";
							}
							response += "---> END OF HELP <---";
							return response;
						}
						#endregion
						#region ping
						else if (commandList[index].Equals("ping"))
						{
							return "pong";
						}
						#endregion
						#region database update profile password
						else if (commandList[index].Equals("database update profile password"))
						{
							string targetUsername = rawCommand.Split(':')[1].Split(' ')[1];
							string newPassword = rawCommand.Split(':')[1].Split(' ')[2];
							Logging.Debug($"Database: Update password for {targetUsername} to {newPassword}");
							bool responseCode = DatabaseManager.UpdateUserPassword(targetUsername, newPassword);
							if (responseCode)
							{
								return "database: ok";
							}
							else
							{
								return "database: failed";
							}
						}
						#endregion
						#region database update profile username
						else if (commandList[index].Equals("database update profile username"))
						{
							string targetUsername = rawCommand.Split(':')[1].Split(' ')[1];
							string newUsername = rawCommand.Split(':')[1].Split(' ')[2];
							Logging.Debug($"Database: Update username for {targetUsername} to {newUsername}");
							bool responseCode = DatabaseManager.UpdateUsername(targetUsername, newUsername);
							if (responseCode)
							{
								return "database: ok";
							}
							else
							{
								return "database: failed";
							}
						}
						#endregion
						#region database update profile permission
						else if (commandList[index].Equals("database update profile permission"))
						{
							string targetUsername = rawCommand.Split(':')[1].Split(' ')[1];
							string newPermission = rawCommand.Split(':')[1].Split(' ')[2];
							Logging.Debug($"Database: Update permission for: {targetUsername} to {newPermission}");
							bool responseCode = DatabaseManager.UpdateUserPermission(targetUsername, newPermission);
							if (responseCode)
							{
								return "database: ok";
							}
							else
							{
								return "database: failed";
							}
						}
						#endregion
						#region database create profile
						else if (commandList[index].Equals("database create profile"))
						{
							// var var var
							string username = rawCommand.Split(':')[1].Split(' ')[1];
							string password = rawCommand.Split(':')[1].Split(' ')[2];
							string permission = rawCommand.Split(':')[1].Split(' ')[3];
							Logging.Debug($"Database: Create new user profile:\nUsername:\t{username}\nPassword:\t{password}\nPermission:\t{permission}");
							bool responseCode = DatabaseManager.CreateUserProfile(username, password, permission);
							if (responseCode)
							{
								return "database: ok";
							}
							else
							{
								return "database: failed";
							}
						}
						#endregion
						#region database delete profile
						else if (commandList[index].Equals("database delete profile"))
						{
							string targetUsername = rawCommand.Split(':')[1].Split(' ')[1];
							Logging.Info($"Delete user {targetUsername}");
							bool responseCode = DatabaseManager.DeleteUserProfile(targetUsername);
							if (responseCode)
							{
								return "database: ok";
							}
							else
							{
								return "database: failed";
							}
						}
						#endregion
						#region database login profile
						else if (commandList[index].Equals("database login profile"))
						{
							string username = rawCommand.Split(':')[1].Split(" ")[1];
							string password = rawCommand.Split(':')[1].Split(" ")[2];
							bool responseCode = DatabaseManager.IsLoginValid(username, password);
							if (responseCode)
							{
								return "database: ok";
							}
							else
							{
								return "database: failed";
							}
						}
						#endregion
						#region database get profile
						else if (commandList[index].Equals("database get profile"))
						{
							string username = rawCommand.Split(':')[1].Split(" ")[1];
							string result = DatabaseManager.GetUserProfile(username);
							return result;
						}
						#endregion
					}
				}
				return "No command found. Try help command";
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return ex.Message;
			}
		}
	}
}
