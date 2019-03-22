<Query Kind="Statements" />

var file = "F:\\SteamGames\\SteamApps\\common\\Touhou Luna Nights\\touhou_luna_nights.exe";
var outf = "F:\\SteamGames\\SteamApps\\common\\Touhou Luna Nights\\touhou_luna_nights.png";
byte[] test = new byte[0xD697A3];
using (BinaryReader reader = new BinaryReader(new FileStream(file, FileMode.Open)))
{
    reader.BaseStream.Seek(0x105BB80, SeekOrigin.Begin);
    reader.Read(test, 0, 0xD697A3);
}
File.WriteAllBytes(outf, test);