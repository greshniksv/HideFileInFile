using System;
using System.Collections.Generic;
using System.Linq;

namespace FileInFileLib.Cipher
{
	public class DataCipher
	{
		public long GetNewPosition(byte hide, byte password)
		{
			return hide + password;
		}

		public byte[] SizeToByte(long size)
		{
			List<byte> outBytes = new List<byte>();
			long current = size;

			for (int i = 7; i >= 1; i--)
			{
				var blockSize = (long)Math.Pow(254, i);
				var result = current / blockSize;
				if (result > 0)
				{
					if (result > 254)
					{
						throw new Exception("Invalid calculation!");
					}

					outBytes.Add((byte)result);
					current -= blockSize * result;
				}
			}

			if (current > 254)
			{
				throw new Exception("Invalid calculation!");
			}

			outBytes.Add((byte)current);
			return outBytes.ToArray();
		}

		public long ByteToSize(byte[] input)
		{
			var items = input.Reverse().ToList();
			long result = 0;

			for (int i = 0; i < items.Count; i++)
			{
				var blockSize = i != 0 ? (long)Math.Pow(254, i) : 1;
				result += blockSize * items[i];
			}

			return result;
		}
	}
}
