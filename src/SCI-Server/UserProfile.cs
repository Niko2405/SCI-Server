using System.Text.Json.Serialization;

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
}
