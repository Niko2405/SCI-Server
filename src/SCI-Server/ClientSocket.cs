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
    class ClientSocket
    {
		private string _serverAddress;
		private int _port;
		private string _data = string.Empty;

		public ClientSocket(string serverAddress, int port)
		{
			_serverAddress = serverAddress;
			_port = port;
		}

		public string SendData(string dataIn)
		{
			_data = dataIn;
			byte[] bytes = new byte[1024];
			string response = string.Empty;
			try
			{
				Logging.Log(Logging.LogLevel.INFO, $"Connecting to {_serverAddress}:{_port}...");
				IPAddress ipAddress = IPAddress.Parse(_serverAddress);
				IPEndPoint remoteEndPoint = new IPEndPoint(ipAddress, _port);
				Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
				try
				{
					sender.Connect(remoteEndPoint);
					if (sender.RemoteEndPoint != null)
					{
						Logging.Log(Logging.LogLevel.INFO, "Socket connected to " + sender.RemoteEndPoint.ToString());
						byte[] bDataToServer = Encoding.UTF8.GetBytes(_data + "<EOF>");
						int bytesSend = sender.Send(bDataToServer);
						int bytesReceive = sender.Receive(bytes);
						response = Encoding.UTF8.GetString(bytes, 0, bytesReceive);
					}
					sender.Shutdown(SocketShutdown.Both);
					sender.Close();
				}
				catch (Exception ex)
				{
					Logging.Log(Logging.LogLevel.ERROR, ex.Message);
					return "No connection";
				}
			}
			catch (Exception ex)
			{
				Logging.Log(Logging.LogLevel.ERROR, ex.Message);
				return "No connection";
			}
			return response;
		}
	}
}
