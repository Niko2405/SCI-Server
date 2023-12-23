using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using SCI_Logger;

namespace SCI_Server
{
	internal class ServerSocket
	{
		private string _interfaceAddress;
		private int _port;
		private string _data = string.Empty;

		/// <summary>
		/// Create new Server Socket
		/// </summary>
		/// <param name="interfaceAddress"></param>
		/// <param name="port"></param>
		public ServerSocket(string interfaceAddress, int port)
		{
			_interfaceAddress = interfaceAddress;
			_port = port;
		}

		/// <summary>
		/// Port of the server socket
		/// </summary>
		public int Port
		{
			get
			{
				return _port;
			}
			set
			{
				_port = value;
			}
		}

		/// <summary>
		/// The interface to which the server listens.
		/// </summary>
		public string InterfaceAddress
		{
			get
			{
				return _interfaceAddress;
			}
			set
			{
				_interfaceAddress = value;
			}
		}

		/// <summary>
		/// Datastream
		/// </summary>
		public string Data
		{
			get
			{
				return _data;
			}
			set
			{
				_data = value;
			}
		}

		/// <summary>
		/// Create an instance of the server
		/// </summary>
		public void StartListener()
		{
			byte[] bytes = new byte[1024];
			IPAddress ipAddress = IPAddress.Parse(_interfaceAddress);

			IPEndPoint localEndPoint = new IPEndPoint(ipAddress, _port);
			Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			try
			{
				listener.Bind(localEndPoint);
				listener.Listen(10);
				Logging.Info($"Server is listening on {ipAddress}:{_port}");
				while (true)
				{
					Socket handler = listener.Accept();
					Logging.PrintHeader("NEW CONNECTION");
					Logging.Info($"Client connection: {handler.RemoteEndPoint}");

					string Data = string.Empty;
					while (true)
					{
						int byteReceive = handler.Receive(bytes);
						Data += Encoding.UTF8.GetString(bytes, 0, byteReceive);
						Logging.Debug($"New bytes receive: {byteReceive}");
						if (Data.IndexOf("<EOF>") > -1)
						{
							break;
						}
					}
					Data = Data.Replace("<EOF>", "");
					Logging.Info($"Received: {Data}");

					string response = NetworkCommands.ProcessCommand(Data);
					Logging.Info($"Response: {response}");

					//Logging.Debug("End of data");

					byte[] bDataToClient = Encoding.UTF8.GetBytes(response);
					handler.Send(bDataToClient);
					handler.Shutdown(SocketShutdown.Both);
					handler.Close();
				}
			}
			catch (Exception ex)
			{
				Logging.Error(ex.Message);
			}
		}
	}
}
