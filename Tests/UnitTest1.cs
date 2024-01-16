using FileInFileLib;
using FileInFileLib.Cipher;

namespace Tests
{
	public class UnitTest1
	{
		[Fact]
		public async Task Test1()
		{
			Engine engine = new Engine();
			await engine.Execute(
				"D:\\Projects\\HideFileInFile\\Tests\\source.txt",
				"D:\\Projects\\HideFileInFile\\Tests\\hide.txt", 
				"1234567890");
		}


		[Fact]
		public void Test2()
		{
			DataCipher cipher = new DataCipher();

			var value = 12459866567;
			var dd = cipher.SizeToByte(value);
			if (cipher.ByteToSize(dd) != value)
			{
				Assert.Fail("asdasd");
			}
		}
	}
}