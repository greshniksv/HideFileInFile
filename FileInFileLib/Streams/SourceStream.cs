using System;
using System.IO;
using System.Threading.Tasks;

namespace FileInFileLib.Streams
{
	public class SourceStream : IDisposable
	{
		private readonly Stream _stream;
		private long length = 0;

		public SourceStream(Stream stream)
		{
			_stream = stream;
			_stream.Position = 10000;
			length = _stream.Length;
		}

		public long Position {
			get { return _stream.Position; }
		}

		public long Seek(long position)
		{
			var newPosition = _stream.Position + position;
			if (newPosition >= length)
			{
				newPosition -= length;
				newPosition += 10000;
			}

			_stream.Position = newPosition;
			return newPosition;
		}

		public byte Read()
		{
			var read = _stream.ReadByte();
			if (read == -1)
			{
				throw new Exception("Reach the end of stream");
			}

			return (byte)read;
		}

		public void Write(byte @byte)
		{
			_stream.WriteByte(@byte);
		}

		public async Task FlushAsync()
		{
			await _stream.FlushAsync();
		}

		public void Dispose()
		{
			_stream?.Dispose();
		}
	}
}
