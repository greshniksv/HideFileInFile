using FileInFileLib.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FileInFileLib.Streams;
using FileInFileLib.Cipher;

namespace FileInFileLib
{
	public class Engine : IEngine
	{
		public event Action<int> Progress;
		private readonly List<long> _usedPositions;

		public Engine()
		{
			_usedPositions = new List<long>();
		}

		public async Task Execute(string sourceFilePath, string hideFilePath, string password)
		{
			var cipher = new DataCipher();
			using (var sourceStream = File.OpenWrite(sourceFilePath))
			using (var hideStream = File.OpenRead(hideFilePath))
			{
				var source = new SourceStream(sourceStream);
				var hide = new HideStream(hideStream);
				var pwd = new PasswordStream(password);
				int progressPercent = 0;

				byte current = 0;
				for (long i = 0; i < hide.Length; i++)
				{
					var curPercent = i * 100 / hide.Length;
					if (progressPercent != curPercent)
					{
						progressPercent = (int)curPercent;
						Progress?.Invoke(progressPercent);
					}

					var pwdCurrent = pwd.GetNextByte();
					var newPos = cipher.GetNewPosition(current, pwdCurrent);

					var realPosition = source.Seek(newPos);
					while (_usedPositions.Contains(realPosition))
					{
						realPosition = source.Seek(1);
					}

					_usedPositions.Add(realPosition);

					current = hide.GetNextByte();
					source.Write(current);
				}

				await source.FlushAsync();
			}
		}

		public async Task ExtractHide(string sourceFilePath, string outFilePath, string password)
		{
			var cipher = new DataCipher();
			using (var sourceStream = File.OpenRead(sourceFilePath))
			using (var outStream = File.OpenWrite(outFilePath))
			{
				var source = new SourceStream(sourceStream);
				var pwd = new PasswordStream(password);

				long pos = 0;
				byte current = 0;
				bool first = true;
				int sizeOfSize = 0;
				List<byte> sizeInfo = new List<byte>();
				long fileSize = 0;
				long stopPosition = 10;
				bool canWrite = false;
				int progressPercent = 0;

				for (int i = 0; i < stopPosition; i++)
				{
					var curPercent = i * 100 / stopPosition;
					if (canWrite && progressPercent != curPercent)
					{
						progressPercent = (int)curPercent;
						Progress?.Invoke(progressPercent);
					}

					var pwdCurrent = pwd.GetNextByte();
					long newPos = cipher.GetNewPosition(current, pwdCurrent);

					var realPosition = source.Seek(newPos);
					while (_usedPositions.Contains(realPosition))
					{
						realPosition = source.Seek(1);
					}

					_usedPositions.Add(realPosition);

					current = source.Read();
					if (canWrite)
					{
						outStream.WriteByte(current);
					}

					if (first)
					{
						sizeOfSize = current;
						first = false;
					}
					else
					{
						if (sizeOfSize != 0)
						{
							sizeInfo.Add(current);
							sizeOfSize--;
						}

						if (sizeOfSize == 0 && fileSize == 0)
						{
							fileSize = cipher.ByteToSize(sizeInfo.ToArray());
							//var pp = Encoding.UTF8.GetString(sizeInfo.ToArray());
							//fileSize = long.Parse(pp);
							stopPosition = fileSize + sizeInfo.Count + 1;
							canWrite = true;
						}
					}
				}

				await outStream.FlushAsync();
			}
		}
	}
}
