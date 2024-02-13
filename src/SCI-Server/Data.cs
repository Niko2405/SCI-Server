using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCI_Server
{
	internal class Data
	{
		public static string DIR_ROOT = "server/";
		public static string DIR_DATA = DIR_ROOT + "data/";
		public static string DIR_CONF = DIR_ROOT + "conf/";

		public static readonly string CRYPT_PASSWORD = "Test123";
		public static readonly string DATAFILE_EXTENTION = ".dat";
	}
}
