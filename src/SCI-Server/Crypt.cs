using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SCI_Server
{
	internal class Crypt
	{
		public static readonly byte[] SALT = [45, 12, 60, 51, 73, 7, 84, 97];
		public static readonly byte[] IV = new byte[16];

		/// <summary>
		/// Verschlüsseln
		/// </summary>
		/// <param name="text"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static string Encrypt(string text)
		{
			byte[] buffer = Encoding.UTF8.GetBytes(text);
			Aes aes = Aes.Create();
			//aes.Padding = PaddingMode.Zeros;
			aes.Key = CreateKey(Data.CRYPT_PASSWORD);
			aes.IV = IV;

			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
				{
					cryptoStream.Write(buffer, 0, buffer.Length);
				}
				return Convert.ToBase64String(memoryStream.ToArray());
			}
		}

		/// <summary>
		/// Entschlüsseln
		/// </summary>
		/// <param name="cryptedText"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static string Decrypt(string cryptedText)
		{
			byte[] buffer = Convert.FromBase64String(cryptedText);
			Aes aes = Aes.Create();
			//aes.Padding = PaddingMode.Zeros;
			aes.Key = CreateKey(Data.CRYPT_PASSWORD);
			aes.IV = IV;

			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
				{
					cryptoStream.Write(buffer, 0, buffer.Length);
					cryptoStream.FlushFinalBlock();
				}
				return Encoding.UTF8.GetString(memoryStream.ToArray());
			}
		}

		/// <summary>
		/// Create Key
		/// </summary>
		/// <param name="password"></param>
		/// <param name="keyBytes"></param>
		/// <returns>Key for en-decryping</returns>
		public static byte[] CreateKey(string password, int keyBytes = 32)
		{
			const int Iterations = 100000;
			var keyGenerator = new Rfc2898DeriveBytes(password, SALT, Iterations, HashAlgorithmName.SHA512);

			return keyGenerator.GetBytes(keyBytes);
		}
	}
}
