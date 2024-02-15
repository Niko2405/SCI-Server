using SCI_Logger;
using System.IO.Ports;

namespace SCI_Server
{
	public class SerialComModule
	{
		public class RS232
		{
			private static bool locked = false;
			private static SerialPort? serialPort;

			public static string[] GetPortNames()
			{
				return SerialPort.GetPortNames();
			}

			/// <summary>
			/// Send command to COM Port
			/// </summary>
			/// <param name="command"></param>
			/// <param name="readLine"></param>
			/// <returns>If readLine is true, repsonse string will be created</returns>
			public static string SendCommand(string command, bool readLine = false)
			{
				if (Config.currentConfig != null)
				{
					string response = string.Empty;
					serialPort = new SerialPort
					{
						PortName = Config.currentConfig.SerialComModulePortName,
						BaudRate = Config.currentConfig.SerialComBaudrate,
						StopBits = StopBits.One,
						DataBits = 8,
						Parity = Parity.None,
						ReadTimeout = 500,
					};
					while (locked)
					{
						Thread.Sleep(1);
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
						catch (TimeoutException)
						{
							Logging.Log(Logging.LogLevel.ERROR, "Timeout by reading value");
						}
						catch (Exception ex)
						{
							Logging.Log(Logging.LogLevel.ERROR, ex.Message);
						}
					}
					return response;
				}
				else
				{
					return "Cannot read config";
				}
			}
		}
	}
}
