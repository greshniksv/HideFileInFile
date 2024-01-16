using FileInFileLib.Interfaces;
using System;
using System.IO;
using System.Text;
using FileInFileLib.Cipher;

namespace FileInFileLib.Streams
{
	public class HideStream : INextByte
	{
		private readonly Stream _stream;
		private readonly byte[] _prefixData;
		private long _position = 0;

		public HideStream(Stream stream)
		{
			_stream = stream;
			var cipher = new DataCipher();
			var sizeInBytes = cipher.SizeToByte(_stream.Length);
			//var pp = Encoding.UTF8.GetBytes(_stream.Length.ToString());
			_prefixData = new byte[sizeInBytes.Length+1];
			_prefixData[0] = (byte)sizeInBytes.Length;
			for (int i = 1; i < sizeInBytes.Length+1; i++)
			{
				_prefixData[i] = sizeInBytes[i-1];
			}
		}

		public long Length {
			get { return _stream.Length + _prefixData.Length; }
		}

		public byte GetNextByte()
		{
			if (_position < _prefixData.Length)
			{
				return _prefixData[_position++];
			}

			var read = _stream.ReadByte();
			if (read == -1)
			{
				throw new Exception("Reach the end of stream");
			}

			
			return (byte)read;
		}
	}
}
