using SCI_Logger;

namespace SCI_Server
{
	internal class Program
	{
		static void Main(string[] args)
		{
			//Console.WriteLine(NetworkManager.Version.GetVersion);
			foreach (string arg in args)
			{
				
			}
			Init.CreateFilesystem();
			Logging.IsDebugEnabled = true;
			DatabaseManager.CheckIntegrity();

			ServerSocket server = new("0.0.0.0", 8080);
			Thread serverThread = new(server.StartListener);
			serverThread.Start();

			Thread.Sleep(1000);
			while (true)
			{
				Console.Write("Local Command>");
				string cmd = Console.ReadLine();
				Netcode.ProcessCommand(cmd);
			}
		}
	}
}
