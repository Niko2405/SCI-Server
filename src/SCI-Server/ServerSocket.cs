using SCI_Logger;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SCI_Server
{
	/// <summary>
	/// Create new Server Socket
	/// </summary>
	/// <param name="interfaceAddress"></param>
	/// <param name="port"></param>
	public class ServerSocket(string ServerAddress, int ServerPort)
	{
		/// <summary>
		/// Create an instance of the server
		/// </summary>
		public void StartListener()
		{
			byte[] bytes = new byte[1024];
			if (ServerAddress != null)
			{
				IPAddress ipAddress = IPAddress.Parse(ServerAddress);
				IPEndPoint localEndPoint = new(ipAddress, ServerPort);
				Socket listener = new(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
				try
				{
					listener.Bind(localEndPoint);
					listener.Listen(10);
					Logging.Log(Logging.LogLevel.INFO, $"Server is listening on {ServerAddress}:{ServerPort}");
					while (true)
					{
						Socket handler = listener.Accept();
						Logging.PrintHeader("NEW CONNECTION");
						Logging.Log(Logging.LogLevel.INFO, $"Client connection: {handler.RemoteEndPoint}");

						string Data = string.Empty;
						while (true)
						{
							int byteReceive = handler.Receive(bytes);
							Data += Encoding.UTF8.GetString(bytes, 0, byteReceive);
							Logging.Log(Logging.LogLevel.DEBUG, $"New bytes receive: {byteReceive}");
							if (Data.IndexOf("<EOF>") > -1)
							{
								break;
							}
						}
						Data = Data.Replace("<EOF>", "");
						Logging.Log(Logging.LogLevel.INFO, $"Received: {Data}");

						//string response = NetworkCommands.ProcessCommand(Data);
						//string response = CommandManager.ProcessCommand(Data);
						string response = "OK";
						Logging.Log(Logging.LogLevel.INFO, $"Response: {response}");

						//Logging.Debug("End of data");

						byte[] bDataToClient = Encoding.UTF8.GetBytes(response);
						handler.Send(bDataToClient);
						handler.Shutdown(SocketShutdown.Both);
						handler.Close();
					}
				}
				catch (Exception ex)
				{
					Logging.Log(Logging.LogLevel.ERROR, ex.Message);
				}
			}
		}
	}
}
