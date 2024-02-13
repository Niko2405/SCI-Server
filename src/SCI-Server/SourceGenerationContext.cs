using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SCI_Server
{
	[JsonSourceGenerationOptions(WriteIndented = true)]
	[JsonSerializable(typeof(UserProfile), GenerationMode = JsonSourceGenerationMode.Metadata)]
	[JsonSerializable(typeof(string))]
	[JsonSerializable(typeof(bool))]
	internal partial class SourceGenerationContext : JsonSerializerContext
	{ }
}
