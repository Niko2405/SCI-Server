using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCI_Server
{
	internal class Init
	{
		public static void CreateFilesystem()
		{
			try
			{
				Directory.CreateDirectory(Data.DIR_ROOT);
				Directory.CreateDirectory(Data.DIR_DATABASE);
				Directory.CreateDirectory(Data.DIR_DATABASE);
			}
			catch (Exception)
			{

				throw;
			}
		}
		public static void DeleteFilesystem() { }
	}
}
