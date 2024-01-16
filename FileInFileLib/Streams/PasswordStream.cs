using FileInFileLib.Interfaces;
using System.Text;

namespace FileInFileLib.Streams
{
	public class PasswordStream : INextByte
	{
		private readonly byte[] _password;
		private long _position;

		public PasswordStream(string password)
		{
			_position = 0;
			_password = Encoding.UTF8.GetBytes(password);
		}

		public byte GetNextByte()
		{
			var current = _position;
			_position++;
			if (_position >= _password.Length)
			{
				_position = 0;
			}

			return _password[current];
		}
	}
}
