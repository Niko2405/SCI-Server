using SCI_Logger;
using System.IO.Ports;
using System.Runtime.Serialization;

namespace SCI_Server
{
	public class SerialComModule
	{
		public class RS232
		{
			public string? SelectedPortName { get; set; }

			public int SelectedBaudrate { get; set; }


			private static bool locked = false;
			private static SerialPort? serialPort;

			public static string[] GetPortNames()
			{
				return SerialPort.GetPortNames();
			}
			public string SendCommand(string command, bool readLine = false)
			{
				string response = string.Empty;
				serialPort = new SerialPort
				{
					PortName = SelectedPortName,
					BaudRate = SelectedBaudrate,
					StopBits = StopBits.One,
					DataBits = 8,
					Parity = 0
				};
				while (locked)
				{
					Thread.Sleep(10);
					Logging.Log(Logging.LogLevel.DEBUG, "Operation is locked");
				}
				if (!serialPort.IsOpen)
				{
					try
					{
						serialPort.Open();
						locked = true;
						serialPort.WriteLine(command);
						if (readLine)
						{
							response = serialPort.ReadLine();
						}
						serialPort.Close();
						locked = false;
					}
					catch (Exception ex)
					{
						Logging.Log(Logging.LogLevel.ERROR, ex.Message);
					}
				}
				return response;
			}
		}
	}
}
