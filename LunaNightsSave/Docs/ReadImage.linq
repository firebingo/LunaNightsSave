<Query Kind="Statements" />

var file = "F:\\SteamGames\\SteamApps\\common\\Touhou Luna Nights\\touhou_luna_nights.exe";
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
var outf = "F:\\SteamGames\\SteamApps\\common\\Touhou Luna Nights\\touhou_luna_nights_2.png";
byte[] test = new byte[0x106588];
using (BinaryReader reader = new BinaryReader(new FileStream(file, FileMode.Open)))
{
    reader.BaseStream.Seek(0x1DC5380, SeekOrigin.Begin);
    reader.Read(test, 0, 0x106588);
}
File.WriteAllBytes(outf, test);