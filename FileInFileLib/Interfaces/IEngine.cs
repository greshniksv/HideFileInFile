using System;
using System.Threading.Tasks;

namespace FileInFileLib.Interfaces
{
	public interface IEngine
	{
		event Action<int> Progress;

		Task Execute(string sourceFilePath, string hideFilePath, string password);
	}
}
