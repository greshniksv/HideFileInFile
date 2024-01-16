// See https://aka.ms/new-console-template for more information

using FileInFileLib;

Console.WriteLine("Hello, World!");


Engine engine = new Engine();

engine.Progress += i =>
{
	Console.Write("\b\b\b\b\b\b");
	Console.Write($"{i}%");
};

//await engine.Execute(
//	"D:\\Projects\\HideFileInFile\\Tests\\nn.mkv",
//	"D:\\Projects\\HideFileInFile\\Tests\\pp.jpg",
//	"1234567890");


//await engine.Execute(
//	"D:\\Projects\\HideFileInFile\\Tests\\source.txt",
//	"D:\\Projects\\HideFileInFile\\Tests\\hide.txt",
//	"1234567890");

//await engine.ExtractHide(
//	"D:\\Projects\\HideFileInFile\\Tests\\source.txt",
//	"D:\\Projects\\HideFileInFile\\Tests\\out.txt",
//	"1234567890");

await engine.ExtractHide(
	"D:\\Projects\\HideFileInFile\\Tests\\nn.mkv",
	"D:\\Projects\\HideFileInFile\\Tests\\out.jpg",
	"1234567890");