using System;
using System.Collections;
using System.Text;

namespace Zeus
{
	/// <summary>
	/// This is rediculous, especially for an open source product. We'll have to change it to support real password based encryption or something like that.
	/// </summary>
	internal class ZeusEncryption
	{
		private ZeusEncryption() {}

		public static string Encrypt(string data) 
		{
			Random rand = new Random();
			ArrayList outBytes = new ArrayList();
			ArrayList p1 = new ArrayList();
			ArrayList p2 = new ArrayList();

			byte[] bytes = Encoding.UTF8.GetBytes(data);
			byte rb = (byte)rand.Next(0xFF);
			byte encryptType = 1;

			// first, Write the Encryption type as the first byte
			// Eventually it will randomly pick from different encrypting types
			outBytes.Add((byte)((encryptType << 2) | (rb & 0xC3)));
			for (int i=0; i < bytes.Length; i++) 
			{
				rb = (byte)rand.Next(0xFF);
				byte b1 = (byte)(bytes[i] ^ 0xFF);
				byte b2 = (byte)((rb & 0x0F) | ((b1 & (byte)0x0F) << 4));
				byte b3 = (byte)((rb & 0xF0) | ((b1 & (byte)0xF0) >> 4));
				
				
				if ((i%2) == 0) 
				{
					p1.Add(b2);
					p1.Add(b3);
				}
				else
				{
					p2.Add(b3);
					p2.Add(b2);
				}
			}
			outBytes.AddRange(p1);
			outBytes.AddRange(p2);

			return Convert.ToBase64String( (byte[])outBytes.ToArray(typeof(byte)) );
		}

		public static string Decrypt(string data) 
		{
			byte[] bytes = Convert.FromBase64String(data);
			
			byte[] outBytes = null;

			// first, Write the Encryption type as the first byte
			// Eventually it will randomly pick from different encrypting types
			byte type = (byte)((bytes[0] >> 2) & 0x0F);

			int c1 = 0, c2 = 1;
			if ((bytes.Length%2) == 1) 
			{
				int half = (bytes.Length-1)/2;
				outBytes = new byte[half];
				for (int i=0; i < bytes.Length-1; i+=2) // len=9, half=4 -> i=0,2,4,6
				{
					byte b1, b2, cb;
					if (i < half) 
					{
						b1 = (byte)((bytes[i+1] & 0xF0) >> 4);
						b2 = (byte)((bytes[i+2] & 0x0F) << 4);
						cb = (byte)((b1 | b2) ^ 0xFF);
						outBytes[c1] = cb;
						c1 += 2;
					}
					else 
					{
						b1 = (byte)((bytes[i+2] & 0xF0) >> 4);
						b2 = (byte)((bytes[i+1] & 0x0F) << 4);
						cb = (byte)((b1 | b2) ^ 0xFF);
						outBytes[c2] = cb;
						c2 += 2;
					}
				}
			}

			
			return Encoding.UTF8.GetString( outBytes );
		}
	}
}
