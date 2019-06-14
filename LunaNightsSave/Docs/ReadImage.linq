<Query Kind="Program" />

static List<FileDef> images = new List<FileDef>()
{
	
}

void Main()
{
	ReadFile();
}

public void ReadFiles(FileDef image)
{
	var file = "G:\\Steam\\SteamApps\\common\\Touhou Luna Nights\\data.win";
	//Sprite Atlas
	//var outf = "F:\\SteamGames\\SteamApps\\common\\Touhou Luna Nights\\touhou_luna_nights.png";
	//byte[] test = new byte[0xD697A3];
	//using (BinaryReader reader = new BinaryReader(new FileStream(file, FileMode.Open)))
	//{
	//    reader.BaseStream.Seek(0x105BB80, SeekOrigin.Begin);
	//    reader.Read(test, 0, 0xD697A3);
	//}
	//File.WriteAllBytes(outf, test);
	
	//Menus etc
	var start = 0x850700;
	var end = 0x8A19C7;
	var length = end - start;
	var outf = "G:\\Steam\\SteamApps\\common\\Touhou Luna Nights\\data_1.png";
	byte[] test = new byte[length];
	using (BinaryReader reader = new BinaryReader(new FileStream(file, FileMode.Open)))
	{
	    reader.BaseStream.Seek(start, SeekOrigin.Begin);
	    reader.Read(test, 0, length);
	}
	File.WriteAllBytes(outf, test);
}

public class FileDef
{
	public int Start;
	public int Length;
	public string FileName;
}

// Define other methods and classes here
