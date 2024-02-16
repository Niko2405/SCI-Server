using SCI_Logger;
using System.Text.Json;

namespace SCI_Server
{
	[Obsolete]
	internal class DataManager
	{
		// TODO: Adding Encrypt and Decrypt
		public static bool AddUserProfile(string username, string password, string permission, string firstname, string lastname, bool isLocked)
		{
			try
			{
				if (File.Exists($"{Config.DIR_DATABASE + username}.json"))
				{
					Logging.Log(Logging.LogLevel.WARN, $"User {Config.DIR_DATABASE + username} already exists. Use RemoveUserProfile()");
					return false;
				}
				var newUser = new UserProfile()
				{
					Username = username,
					Password = password,
					Permission = permission,
					FirstName = firstname,
					LastName = lastname,
					IsLocked = isLocked
				};
				string jsonString = JsonSerializer.Serialize(newUser, Config.JsonOptions);
				File.WriteAllText($"{Config.DIR_DATABASE + username}.json", jsonString);
				return true;
			}
			catch (Exception ex)
			{
				Logging.Log(Logging.LogLevel.ERROR, ex.Message);
				return false;
			}
		}
		public static bool RemoveUserProfile(string username)
		{
			try
			{
				if (username == "root")
				{
					Logging.Log(Logging.LogLevel.WARN, "Cannot remove system profile");
					return false;
				}
				if (File.Exists(Config.DIR_DATABASE + username + ".json"))
				{
					File.Delete(Config.DIR_DATABASE + username + ".json");
					Logging.Log(Logging.LogLevel.INFO, $"UserProfile of {username} deleted");
					return true;
				}
				Logging.Log(Logging.LogLevel.WARN, $"UserProfile {username} not found");
				return false;
			}
			catch (Exception ex)
			{
				Logging.Log(Logging.LogLevel.ERROR, ex.Message);
				return false;
			}
		}

		public static bool ChangeUsername(string username, string newUsername)
		{
			try
			{
				if (username == "root")
				{
					Logging.Log(Logging.LogLevel.WARN, "Cannot edit system profile");
					return false;
				}
				if (File.Exists(Config.DIR_DATABASE + username + ".json"))
				{
					// read data
					string jsonString = File.ReadAllText(Config.DIR_DATABASE + username + ".json");
					var userProfile = JsonSerializer.Deserialize<UserProfile>(jsonString, Config.JsonOptions);
					if (userProfile != null)
					{
						Logging.Log(Logging.LogLevel.INFO, $"Update username ({userProfile.Username} => {newUsername})");
						userProfile.Username = newUsername;
						jsonString = JsonSerializer.Serialize(userProfile, Config.JsonOptions);
						File.WriteAllText(Config.DIR_DATABASE + newUsername + ".json", jsonString);

						Logging.Log(Logging.LogLevel.INFO, $"Delete old profile of {username}");
						File.Delete(Config.DIR_DATABASE + username + ".json");
						return true;
					}
				}
				return false;
			}
			catch (Exception ex)
			{
				Logging.Log(Logging.LogLevel.ERROR, ex.Message);
				return false;
			}
		}

		public static bool ChangePassword(string username, string newPassword)
		{
			try
			{
				if (username == "root")
				{
					Logging.Log(Logging.LogLevel.WARN, "Cannot edit system profile");
					return false;
				}
				if (File.Exists(Config.DIR_DATABASE + username + ".json"))
				{
					// read data
					string jsonString = File.ReadAllText(Config.DIR_DATABASE + username + ".json");
					var userProfile = JsonSerializer.Deserialize<UserProfile>(jsonString, Config.JsonOptions);
					if (userProfile != null)
					{
						Logging.Log(Logging.LogLevel.INFO, $"Update password ({userProfile.Password} => {newPassword})");
						userProfile.Password = newPassword;
						jsonString = JsonSerializer.Serialize(userProfile, Config.JsonOptions);

						// write new data
						File.WriteAllText(Config.DIR_DATABASE + username + ".json", jsonString);

						Logging.Log(Logging.LogLevel.INFO, $"Change password profile of {username}");
						return true;
					}
				}
				return false;
			}
			catch (Exception ex)
			{
				Logging.Log(Logging.LogLevel.ERROR, ex.Message);
				return false;
			}
		}
		public static bool ChangePermission(string username, string newPermission)
		{
			try
			{
				if (username == "root")
				{
					Logging.Log(Logging.LogLevel.WARN, "Cannot edit system profile");
					return false;
				}
				if (File.Exists(Config.DIR_DATABASE + username + ".json"))
				{
					// read data
					string jsonString = File.ReadAllText(Config.DIR_DATABASE + username + ".json");
					var userProfile = JsonSerializer.Deserialize<UserProfile>(jsonString, Config.JsonOptions);
					if (userProfile != null)
					{
						Logging.Log(Logging.LogLevel.INFO, $"Update permission ({userProfile.Password} => {newPermission})");
						userProfile.Permission = newPermission;
						jsonString = JsonSerializer.Serialize(userProfile, Config.JsonOptions);

						// write new data
						File.WriteAllText(Config.DIR_DATABASE + username + ".json", jsonString);

						Logging.Log(Logging.LogLevel.INFO, $"Change permission profile of {username}");
						return true;
					}
				}
				return false;
			}
			catch (Exception ex)
			{
				Logging.Log(Logging.LogLevel.ERROR, ex.Message);
				return false;
			}
		}
		public static bool IsLoginValid(string username, string password)
		{
			try
			{
				if (File.Exists(Config.DIR_DATABASE + username + ".json"))
				{
					// read data
					string jsonString = File.ReadAllText(Config.DIR_DATABASE + username + ".json");
					var userProfile = JsonSerializer.Deserialize<UserProfile>(jsonString, Config.JsonOptions);
					if (userProfile != null)
					{
						//Logging.Log(Logging.LogLevel.INFO, $"Update permission ({userProfile.Password} => {newPermission})");
						if (userProfile.Username == username && userProfile.Password == password)
						{
							Logging.Log(Logging.LogLevel.INFO, $"Login for ({userProfile.Username}) is correct");
							return true;
						}
						else
						{
							Logging.Log(Logging.LogLevel.ERROR, $"Login for ({username} is invalid)");
							return false;
						}
					}
				}
				Logging.Log(Logging.LogLevel.ERROR, $"Login for ({username} is invalid)");
				return false;
			}
			catch (Exception ex)
			{
				Logging.Log(Logging.LogLevel.ERROR, ex.Message);
				return false;
			}
		}
	}
}
