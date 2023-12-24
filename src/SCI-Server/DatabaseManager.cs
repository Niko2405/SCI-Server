using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SCI_Logger;

namespace SCI_Server
{
	internal class DatabaseManager
	{
		private static readonly string DB_USERS = "users.db";
		private static readonly string DB_SYSTEM = "system.db";

		private static readonly string DEFAULT_USERNAME = "root";
		private static readonly string DEFAULT_PASSWORD = "root";

		/// <summary>
		/// Check the databases for corruption
		/// </summary>
		public static void CheckIntegrity()
		{
			// check users.db
			if (!System.IO.File.Exists(Data.DIR_DATABASE + DB_USERS))
			{
				Logging.Info($"Create new database: {DB_USERS}...");
				SQLiteConnection connection = new SQLiteConnection($"Data Source={Data.DIR_DATABASE + DB_USERS}; Version = 3; New = True;");
				try
				{
					connection.Open();
					var sqlCommand = connection.CreateCommand();
					Logging.Info("Creating profile table...");
					sqlCommand.CommandText = @"
						CREATE TABLE profiles (
							id			INTEGER		PRIMARY KEY ON CONFLICT ROLLBACK AUTOINCREMENT,
							username	TEXT		NOT NULL UNIQUE ON CONFLICT FAIL,
							password	TEXT		NOT NULL,
							permission	TEXT		NOT NULL,
							birthday	TEXT		NOT NULL,
							country		TEXT		NOT NULL,
							state		TEXT		NOT NULL,
							age			INTEGER		NOT NULL,
							locked		INTEGER		NOT NULL DEFAULT (0)
						);
					";
					sqlCommand.ExecuteNonQuery();

					Logging.Info("Creating default root profile...");
					sqlCommand.CommandText = $"INSERT INTO profiles (username, password, permission, birthday, country, state, age, locked) VALUES('{DEFAULT_USERNAME}', '{DEFAULT_PASSWORD}', 'root', '01.01.1998', 'Germany', 'Hessen', 0, 0)";
					sqlCommand.ExecuteNonQuery();
					connection.Close();
				}
				catch (Exception ex)
				{
					Logging.Error(ex.Message);
				}
			}

			// Check system.db
			if (!System.IO.File.Exists(Data.DIR_DATABASE + DB_SYSTEM))
			{
				Logging.Info($"Create new database: {DB_SYSTEM}...");
				SQLiteConnection connection = new SQLiteConnection($"Data Source={Data.DIR_DATABASE + DB_SYSTEM}; Version = 3; New = True;");
				connection.Open();
				// create tables
				connection.Close();
			}

			// TEST DATABASE USERS
			#region integrity_checks
			Logging.Info($"Database: {Data.DIR_DATABASE + DB_USERS} detected. Check integrity...");
			try
			{
				SQLiteConnection connection = new SQLiteConnection($"Data Source={Data.DIR_DATABASE + DB_SYSTEM};");
				connection.Open();
				var sqlCommand = connection.CreateCommand();

				sqlCommand.CommandText = "PRAGMA integrity_check;";
				sqlCommand.ExecuteNonQuery();
				connection.Close();
			}
			catch (Exception ex)
			{
				Logging.Error(ex.Message);
			}

			// TEST DATABASE USERS
			Logging.Info($"Database: {Data.DIR_DATABASE + DB_SYSTEM} detected. Check integrity...");
			try
			{
				SQLiteConnection connection = new SQLiteConnection($"Data Source={Data.DIR_DATABASE + DB_SYSTEM};");
				connection.Open();
				var sqlCommand = connection.CreateCommand();

				sqlCommand.CommandText = "PRAGMA integrity_check;";
				sqlCommand.ExecuteNonQuery();
				connection.Close();
			}
			catch (Exception ex)
			{
				Logging.Error(ex.Message);
			}
			#endregion
		}

		/// <summary>
		/// Create new User profile
		/// </summary>
		/// <param name="username">Username</param>
		/// <param name="password">Password</param>
		/// <param name="permission">Permission</param>
		/// <returns>true if operation success, false if failed</returns>
		public static bool CreateUserProfile(string username, string password, string permission)
		{
			SQLiteConnection connection = new SQLiteConnection($"Data Source={Data.DIR_DATABASE + DB_USERS};");
			try
			{
				connection.Open();
				var sqlCommand = connection.CreateCommand();
				sqlCommand.CommandText = $"INSERT INTO profiles (username, password, permission, birthday, country, state, age, locked) VALUES ('{username}', '{password}', '{permission}', '01.01.1998', 'Germany', 'Hessen', 20, 0);";
				sqlCommand.ExecuteNonQuery();
				return true;
			}
			catch (Exception ex)
			{
				Logging.Error(ex.Message);
				return false;
			}
		}

		/// <summary>
		/// Delete user profile
		/// </summary>
		/// <param name="username"></param>
		/// <returns>true if operation success, false if failed</returns>
		public static bool DeleteUserProfile(string username)
		{
			if (username == "root")
			{
				Logging.Warn("Cannot delete root");
				return false;
			}
			else
			{
				SQLiteConnection connection = new SQLiteConnection($"Data Source={Data.DIR_DATABASE + DB_USERS};");
				try
				{
					connection.Open();
					var sqlCommand = connection.CreateCommand();
					sqlCommand.CommandText = $"DELETE FROM profiles WHERE profiles.username = '{username}';";
					sqlCommand.ExecuteNonQuery();
					return true;
				}
				catch (Exception ex)
				{
					Logging.Error(ex.Message);
					return false;
				}
			}
		}

		/// <summary>
		/// Update username
		/// </summary>
		/// <param name="targetUsername">Target Username</param>
		/// <param name="newUsername">new Username</param>
		/// <returns>true if operation success, false if failed</returns>
		public static bool UpdateUsername(string targetUsername, string newUsername)
		{
			if (targetUsername == "root")
			{
				Logging.Warn("Not allowed");
				return false;
			}
			SQLiteConnection connection = new SQLiteConnection($"Data Source={Data.DIR_DATABASE + DB_USERS};");
			try
			{
				connection.Open();
				var sqlCommand = connection.CreateCommand();
				sqlCommand.CommandText = "SELECT profiles.username FROM profiles";
				string? response = sqlCommand.ExecuteScalar().ToString();
				if (response != null)
				{
					if (response.Contains(targetUsername))
					{
						sqlCommand.CommandText = $"UPDATE profiles SET username = '{newUsername}' WHERE username = '{targetUsername}';";
						sqlCommand.ExecuteNonQuery();
						return true;
					}
					else
					{
						Logging.Info($"Username: {targetUsername} doesn't exists");
					}
				}
				return false;
			}
			catch (Exception ex)
			{
				Logging.Error(ex.Message);
				return false;
			}
		}

		/// <summary>
		/// Update user permissions
		/// </summary>
		/// <param name="targetUsername">Target Username</param>
		/// <param name="newPermission">new Permission</param>
		/// <returns>true if operation success, false if failed</returns>
		public static bool UpdateUserPermission(string targetUsername, string newPermission)
		{
			SQLiteConnection connection = new SQLiteConnection($"Data Source={Data.DIR_DATABASE + DB_USERS};");
			try
			{
				connection.Open();
				var sqlCommand = connection.CreateCommand();
				sqlCommand.CommandText = "SELECT profiles.username FROM profiles";
				string? response = sqlCommand.ExecuteScalar().ToString();
				if (response != null)
				{
					if (response.Contains(targetUsername))
					{
						sqlCommand.CommandText = $"UPDATE profiles SET permission = '{newPermission}' WHERE username = '{targetUsername}';";
						sqlCommand.ExecuteNonQuery();
						return true;
					}
					else
					{
						Logging.Info($"Username: {targetUsername} doesn't exists");
					}
				}
				return false;
			}
			catch (Exception ex)
			{
				Logging.Error(ex.Message);
				return false;
			}
		}

		/// <summary>
		/// Update user password
		/// </summary>
		/// <param name="targetUsername">Target Username</param>
		/// <param name="newPassword">new password</param>
		/// <returns>true if operation success, false if failed</returns>
		public static bool UpdateUserPassword(string targetUsername, string newPassword)
		{
			SQLiteConnection connection = new SQLiteConnection($"Data Source={Data.DIR_DATABASE + DB_USERS};");
			try
			{
				connection.Open();
				var sqlCommand = connection.CreateCommand();
				sqlCommand.CommandText = "SELECT profiles.username FROM profiles";
				string? response = sqlCommand.ExecuteScalar().ToString();
				if (response != null)
				{
					if (response.Contains(targetUsername))
					{
						sqlCommand.CommandText = $"UPDATE profiles SET password = '{newPassword}' WHERE username = '{targetUsername}';";
						sqlCommand.ExecuteNonQuery();
						return true;
					}
					else
					{
						Logging.Info($"Username: {targetUsername} doesn't exists");
					}
				}
				return false;
			}
			catch (Exception ex)
			{
				Logging.Error(ex.Message);
				return false;
			}
		}
		//SELECT* FROM profiles WHERE profiles.username = 'root';

		public static string GetUserProfile(string targetUsername)
		{
			string result = string.Empty;
			string[] response = Array.Empty<string>();

			SQLiteConnection connection = new SQLiteConnection($"Data Source={Data.DIR_DATABASE + DB_USERS};");
			try
			{
				connection.Open();
				var sqlCommand = connection.CreateCommand();
				sqlCommand.CommandText = $"SELECT * FROM profiles WHERE profiles.username = '{targetUsername}'";
				SQLiteDataReader reader = sqlCommand.ExecuteReader();
				while (reader.Read())
				{
					// id
					result += reader.GetInt32(0).ToString() + Environment.NewLine;
					// username
					result += reader.GetString(1) + Environment.NewLine;
					// password
					result += reader.GetString(2) + Environment.NewLine;
					// permission
					result += reader.GetString(3) + Environment.NewLine;
					// birthday
					result += reader.GetString(4) + Environment.NewLine;
					// country
					result += reader.GetString(5) + Environment.NewLine;
					// state
					result += reader.GetString(6) + Environment.NewLine;
					// age
					result += reader.GetInt32(7).ToString() + Environment.NewLine;
					// locked
					result += reader.GetInt32(8).ToString();
				}
				//response = dbUsername + Environment.NewLine + dbPassword + Environment.NewLine + dbPermission + Environment.NewLine + dbBirthday + Environment.NewLine + dbCountry + Environment.NewLine + dbState + Environment.NewLine + dbAge + Environment.NewLine + dbLocked;
				//Log.Debug($"Userprofile:\ndbUsername:\t{dbUsername}\ndbPassword:\t{dbPassword}\ndbPermission:\t{dbPermission}\ndbBirthday:\t{dbBirthday}\ndbCountry:\t{dbCountry}\ndbState:\t{dbState}\ndbAge:\t{dbAge}\ndbLocked:\t{dbLocked}");
				return result;
			}
			catch (Exception ex)
			{
				result += ex.Message + Environment.NewLine;
				Logging.Error(ex.Message);
				return result;
			}
		}
		/// <summary>
		/// Check if login is valid
		/// </summary>
		/// <param name="username">Username</param>
		/// <param name="password">Password</param>
		/// <returns>true if valid and false for invalid</returns>
		public static bool IsLoginValid(string username, string password)
		{
			string dbUsername = string.Empty;
			string dbPassword = string.Empty;

			SQLiteConnection connection = new SQLiteConnection($"Data Source={Data.DIR_DATABASE + DB_USERS};");
			try
			{
				connection.Open();
				var sqlCommand = connection.CreateCommand();

				// search for username
				sqlCommand.CommandText = $"SELECT * FROM profiles WHERE profiles.username = '{username}';";
				SQLiteDataReader reader = sqlCommand.ExecuteReader();
				while (reader.Read())
				{
					dbUsername = reader.GetString(1);
					dbPassword = reader.GetString(2);
				}
				Console.WriteLine($"dbUsername:\t{dbUsername}\tdbPassword:\t{dbPassword}\nUsername:\t{username}\tPassword:\t{password}");
				if (dbUsername == username && dbPassword == password)
				{
					Logging.Info("Login is valid");
					return true;
				}
				Logging.Info("Login is invalid");
				return false;
			}
			catch (Exception ex)
			{
				Logging.Error(ex.Message);
				return false;
			}
		}
	}
}
