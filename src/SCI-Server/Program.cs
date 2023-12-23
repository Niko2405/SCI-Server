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
				if (arg == "--client")
				{
					ClientSocket client = new("127.0.0.1", 8080);
					while (true)
					{
						Console.Write(">");
						string cmd = Console.ReadLine();
						Logging.Info(client.SendData(cmd));
					}
					
				}
			}
			Init.CreateFilesystem();
			DatabaseManager.CheckIntegrity();

			ServerSocket server = new("0.0.0.0", 8080);
			Thread serverThread = new Thread(server.StartListener);
			serverThread.Start();
		}
	}
}
