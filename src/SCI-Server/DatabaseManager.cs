using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using System.Linq;
using System.Text;

using SCI_Logger;
using System.Net.Sockets;
using System.Security;

namespace SCI_Server
{
	internal class DatabaseManager
	{
		public static readonly string VERSION = "1.0";
		public static readonly string DATABASE_SYSTEM = Config.DIR_DATABASE + "system.db";

		/// <summary>
		/// Create user table
		/// </summary>
		/// <returns>True or false if operation successful</returns>
		public static bool CreateUserTable()
		{
			try
			{
				Logging.Log(Logging.LogLevel.INFO, "Check user table");
				using var connection = new SqliteConnection($"Data Source={DATABASE_SYSTEM}");
				connection.Open();
				var command = connection.CreateCommand();
				command.CommandText = @"
				CREATE TABLE IF NOT EXISTS users (
					id			INTEGER	NOT NULL PRIMARY KEY AUTOINCREMENT,
					username	TEXT NOT NULL UNIQUE ON CONFLICT FAIL,
					password	TEXT NOT NULL,
					permission	TEXT NOT NULL,
					firstname	TEXT NOT NULL,
					lastname	TEXT NOT NULL,
					locked		INTEGER NOT NULL
				)";
				command.ExecuteNonQuery();
				connection.Close();
				return true;
			}
			catch (Exception ex)
			{
				Logging.Log(Logging.LogLevel.ERROR, ex.Message);
				return false;
			}
		}

		/// <summary>
		/// Add User to system
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <param name="firstname"></param>
		/// <param name="lastname"></param>
		/// <param name="permission"></param>
		/// <param name="locked"></param>
		/// <returns>True or false if operation successful</returns>
		public static bool AddUser(string username, string password, string firstname, string lastname, string permission, int locked)
		{
			// check if user already exists in the database
			List<string> dbUsers = GetUsernames();
			for (int i = 0; i < dbUsers.Count; i++)
			{
				if (dbUsers[i] == username)
				{
					Logging.Log(Logging.LogLevel.WARN, $"User '{username}' already exists in the database");
					return false;
				}
			}

			Logging.Log(Logging.LogLevel.INFO, $"Add user [{username}]");
			try
			{
				using var connection = new SqliteConnection($"Data Source={DATABASE_SYSTEM}");
				connection.Open();
				var command = connection.CreateCommand();
				command.CommandText = $"INSERT INTO users (username, password, firstname, lastname, permission, locked) VALUES ('{username}', '{password}', '{firstname}', '{lastname}', '{permission}', '{locked}')";
				command.ExecuteNonQuery();
				connection.Close();
				return true;
			}
			catch (Exception ex)
			{
				Logging.Log(Logging.LogLevel.ERROR, ex.Message);
				return false;
			}
		}

		/// <summary>
		/// Delete User from system
		/// </summary>
		/// <param name="username"></param>
		/// <returns>True or false if operation successful</returns>
		public static bool RemoveUser(string username)
		{
			try
			{
				if (username != "root")
				{
					using var connection = new SqliteConnection($"Data Source={DATABASE_SYSTEM}");
					connection.Open();
					var command = connection.CreateCommand();
					command.CommandText = $"DELETE FROM users WHERE users.username = '{username}'";
					command.ExecuteNonQuery();
					connection.Close();
					return true;
				}
				Logging.Log(Logging.LogLevel.WARN, "It's not allowed to remove root user");
				return false;
			}
			catch (Exception ex)
			{
				Logging.Log(Logging.LogLevel.ERROR, ex.Message);
				return false;
			}
		}

		public static bool SetFirstname(string username, string newFirstname)
		{
			try
			{
				if (username != "root")
				{
					using var connection = new SqliteConnection($"Data Source={DATABASE_SYSTEM}");
					connection.Open();
					var command = connection.CreateCommand();
					command.CommandText = $"SELECT users.username, users.firstname FROM users WHERE users.username = '{username}';";
					string? result = command.ExecuteScalar().ToString();
					if (result != null)
					{
						if (result.Contains(username))
						{
							command.CommandText = $"UPDATE users SET firstname = '{newFirstname}' WHERE users.username = '{username}';";
							command.ExecuteNonQuery();
							connection.Close();
							Logging.Log(Logging.LogLevel.INFO, $"Set Firstname for [{username}] to {newFirstname}");
							return true;
						}
						else
						{
							Logging.Log(Logging.LogLevel.WARN, $"User [{username}] not found");
							connection.Close();
						}
						return false;
					}
				}
				Logging.Log(Logging.LogLevel.WARN, "It's not allowed to edit firstname for root user");
				return false;
			}
			catch (Exception ex)
			{
				Logging.Log(Logging.LogLevel.ERROR, ex.Message);
				return false;
			}
		}
		public static bool SetUsername(string username, string newUsername)
		{
			try
			{
				if (username != "root")
				{
					using var connection = new SqliteConnection($"Data Source={DATABASE_SYSTEM}");
					connection.Open();
					var command = connection.CreateCommand();
					command.CommandText = $"SELECT users.username FROM users;";
					string? result = command.ExecuteScalar().ToString();
					if (result != null)
					{
						if (result.Contains(username))
						{
							command.CommandText = $"UPDATE users SET username = '{newUsername}' WHERE username = '{username}';";
							command.ExecuteNonQuery();
							connection.Close();
							return true;
						}
						else
						{
							Logging.Log(Logging.LogLevel.WARN, $"User [{username}] not found");
							connection.Close();
						}
						return false;
					}
				}
				Logging.Log(Logging.LogLevel.WARN, "It's not allowed to edit username for root user");
				return false;
			}
			catch (Exception ex)
			{
				Logging.Log(Logging.LogLevel.ERROR, ex.Message);
				return false;
			}
		}


		/// <summary>
		/// Get all usernames
		/// </summary>
		/// <returns>List of usernames</returns>
		public static List<string> GetUsernames()
		{
			List<string> users = [];
			try
			{
				using var connection = new SqliteConnection($"Data Source={DATABASE_SYSTEM}");
				connection.Open();
				var command = connection.CreateCommand();
				command.CommandText = "SELECT users.username FROM users;";
				SqliteDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					users.Add(reader.GetString(0));
				}
				reader.Close();
				connection.Close();

				Logging.Log(Logging.LogLevel.DEBUG, $"There are currently {users.Count} users registered");
				return users;
			}
			catch (Exception ex)
			{
				Logging.Log(Logging.LogLevel.ERROR, ex.Message);
				return users;
			}
		}
		public static bool CreateConfigTable()
		{
			return true;
		}
		public static bool CreateOtherTables()
		{
			return true;
		}
	}
}
