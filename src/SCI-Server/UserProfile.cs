using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SCI_Server
{
	internal class UserProfile
	{
		public required string Username { get; set; }
		public required string FirstName { get; set; }
		public required string LastName { get; set; }
		public required string Password { get; set; }
		public required string Permission { get; set; }
		public required bool IsLocked { get; set; }
	}

	/// <summary>
	/// AOT JSON
	/// </summary>
	[JsonSourceGenerationOptions(WriteIndented = true)]
	[JsonSerializable(typeof(UserProfile), GenerationMode = JsonSourceGenerationMode.Metadata)]
	[JsonSerializable(typeof(string))]
	[JsonSerializable(typeof(bool))]
	internal partial class UserProfileContext : JsonSerializerContext
	{ }
}
