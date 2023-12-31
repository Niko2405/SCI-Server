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
			Logging.PrintHeader($"DATABASE INIT [{DB_USERS}]");
			// check users.db
			if (!System.IO.File.Exists(Data.DIR_DATABASE + DB_USERS))
			{
				Logging.Info($"Create new database: {DB_USERS}...");
				SQLiteConnection connection = new($"Data Source={Data.DIR_DATABASE + DB_USERS}; Version = 3; New = True;");
				try
				{
					connection.Open();
					var sqlCommand = connection.CreateCommand();
					Logging.Info("Creating profile table...");
					sqlCommand.CommandText =
						@"
							CREATE TABLE profiles (
								id			INTEGER		PRIMARY KEY ON CONFLICT ROLLBACK AUTOINCREMENT,
								username	TEXT		NOT NULL UNIQUE ON CONFLICT FAIL,
								firstname	TEXT		NOT NULL,
								lastname	TEXT		NOT NULL,
								password	TEXT		NOT NULL,
								permission	TEXT		NOT NULL,
								birthday	TEXT		NOT NULL,
								country		TEXT		NOT NULL,
								state		TEXT		NOT NULL,
								city		TEXT		NOT NULL,
								age			INTEGER		NOT NULL,
								userlocked	INTEGER		NOT NULL DEFAULT (0)
						);
					";
					sqlCommand.ExecuteNonQuery();

					Logging.Info("Creating default root profile...");
					sqlCommand.CommandText = $"INSERT INTO profiles (username, firstname, lastname, password, permission, birthday, country, state, city, age, userlocked) VALUES('{DEFAULT_USERNAME}', 'Max', 'Mustermann','{DEFAULT_PASSWORD}', 'root', '01.01.1998', 'Germany', 'Hessen', 'Frankfurt', 0, 0)";
					sqlCommand.ExecuteNonQuery();
					connection.Close();
				}
				catch (Exception ex)
				{
					Logging.Error(ex.Message);
				}
			}
			Logging.Info("OK");

			Logging.PrintHeader($"DATABASE INIT [{DB_SYSTEM}]");
			// Check system.db
			if (!System.IO.File.Exists(Data.DIR_DATABASE + DB_SYSTEM))
			{
				try
				{
					Logging.Info($"Create new database: {DB_SYSTEM}...");
					SQLiteConnection connection = new($"Data Source={Data.DIR_DATABASE + DB_SYSTEM}; Version = 3; New = True;");
					connection.Open();
					// create tables
					var sqlCommand = connection.CreateCommand();
					Logging.Info("Creating keys table...");
					sqlCommand.CommandText =
						@"
						CREATE TABLE keys (
							id				INTEGER		PRIMARY KEY ON CONFLICT ROLLBACK AUTOINCREMENT,
							accesscode		INTEGER		NOT NULL UNIQUE ON CONFLICT FAIL,
							secruitykey		INTEGER		NOT NULL UNIQUE ON CONFLICT FAIL
						);
					";
					sqlCommand.ExecuteNonQuery();

					Logging.Info("Creating default keys...");
					sqlCommand.CommandText = $"INSERT INTO keys (accesscode, secruitykey) VALUES(10, 20)";
					sqlCommand.ExecuteNonQuery();
					connection.Close();
				}
				catch (Exception ex)
				{
					Logging.Error(ex.Message);
				}
			}
			Logging.Info("OK");

			// TEST DATABASE USERS
			Logging.PrintHeader($"DATABASE INTEGRITY TEST");
			#region integrity_checks
			Logging.Info($"Database: {Data.DIR_DATABASE + DB_USERS} detected. Check integrity...");
			try
			{
				SQLiteConnection connection = new($"Data Source={Data.DIR_DATABASE + DB_SYSTEM};");
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
			Logging.Info("OK");

			// TEST DATABASE SYSTEM
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
			Logging.Info("OK");
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
				sqlCommand.CommandText = $"INSERT INTO profiles (username, firstname, lastname, password, permission, birthday, country, state, city, age, locked) VALUES ('{username}', 'new firstname', 'new lastname', '{password}', '{permission}', '01.01.1998', 'Germany', 'Hessen', 'Frankfurt', 20, 0);";
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
			SQLiteConnection connection = new($"Data Source={Data.DIR_DATABASE + DB_USERS};");
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
			string[] response = [];

			SQLiteConnection connection = new($"Data Source={Data.DIR_DATABASE + DB_USERS};");
			try
			{
				connection.Open();
				var sqlCommand = connection.CreateCommand();
				sqlCommand.CommandText = $"SELECT * FROM profiles WHERE profiles.username = '{targetUsername}'";
				SQLiteDataReader reader = sqlCommand.ExecuteReader();
				reader.Read();
				object[] data = new object[12];
				int quant = reader.GetValues(data);
				for (int i = 0; i < quant; i++)
				{
					result += data[i] + "\n";
				}
				reader.Close();
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
		/// Check if the login is valid. Username and password have to match. AccessCode and Secruity have to also match in the tabel
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <param name="accesscode"></param>
		/// <param name="secruitykey"></param>
		/// <returns>True if login is valid, False when is invalid</returns>
		public static bool IsLoginValid(string username, string password, int accesscode, int secruitykey)
		{
			string dbUsername = string.Empty;
			string dbPassword = string.Empty;

			int dbAccessCode = GetAccessCode(secruitykey);
			int dbSecruityKey = GetSecruityKey(accesscode);
			
			SQLiteConnection connection = new($"Data Source={Data.DIR_DATABASE + DB_USERS};");
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
					dbPassword = reader.GetString(4);
				}
				Logging.Debug($"\ndbUsername:\t{dbUsername}\tdbPassword:\t{dbPassword}\nUsername:\t{username}\tPassword:\t{password}\ndbAccessCode:\t{dbAccessCode}\tdbSecruityKey:\t{dbSecruityKey}\nAccessCode:\t{accesscode}\tSecruityKey:\t{secruitykey}");
				if (dbUsername == username && dbPassword == password)
				{
					if (dbSecruityKey == secruitykey && dbAccessCode == accesscode)
					{
						Logging.Info("Login is valid");
						return true;
					}
					Logging.Error("Code / Key is invalid");
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

		/// <summary>
		/// Search for the correct AccessCode by using the given SecruityKey
		/// </summary>
		/// <param name="secruitykey"></param>
		/// <returns>Gives the AccessCode that's equals to the SecruityKey</returns>
		public static int GetAccessCode(int secruitykey)
		{
			int result = -1;
			SQLiteConnection connection = new($"Data Source={Data.DIR_DATABASE + DB_SYSTEM};");
			try
			{
				connection.Open();
				var sqlCommand = connection.CreateCommand();
				sqlCommand.CommandText = $"SELECT * FROM keys WHERE keys.secruitykey = {secruitykey}";
				SQLiteDataReader reader = sqlCommand.ExecuteReader();
				while (reader.Read())
				{
					// get the dbAccessCode
					result = reader.GetInt16(1);
				}
				reader.Close();
				return result;
			}
			catch (Exception ex)
			{
				Logging.Error(ex.Message);
				return result;
			}
		}

		/// <summary>
		/// Search for the correct SecruityKey by using the given AccessCode
		/// </summary>
		/// <param name="accesscode"></param>
		/// <returns>Gives the SecruityKey that's equals to the AccessCode</returns>
		public static int GetSecruityKey(int accesscode)
		{
			int result = -1;
			SQLiteConnection connection = new($"Data Source={Data.DIR_DATABASE + DB_SYSTEM};");
			try
			{
				connection.Open();
				var sqlCommand = connection.CreateCommand();
				sqlCommand.CommandText = $"SELECT * FROM keys WHERE keys.accesscode = {accesscode}";
				SQLiteDataReader reader = sqlCommand.ExecuteReader();
				while (reader.Read())
				{
					// get the dbSecruityCode
					result = reader.GetInt16(2);
				}
				reader.Close();
				return result;
			}
			catch (Exception ex)
			{
				Logging.Error(ex.Message);
				return result;
			}
		}
		public static string[] GetAllUserProfiles()
		{
			string[] users = [];
			int counter = 0;
			SQLiteConnection connection = new($"Data Source={Data.DIR_DATABASE + DB_USERS};");
			try
			{
				connection.Open();
				var sqlCommand = connection.CreateCommand();

				sqlCommand.CommandText = $"SELECT * FROM profiles;";
				SQLiteDataReader reader = sqlCommand.ExecuteReader();
				while (reader.Read())
				{
					users[counter] = reader.GetString(counter + 1);
					counter++;
				}
				Console.WriteLine($"Länge {users.Length}");
				return users;
			}
			catch (Exception)
			{
				return users;
			}
		}
	}
}
