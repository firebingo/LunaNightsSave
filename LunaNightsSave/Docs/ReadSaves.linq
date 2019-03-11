<Query Kind="Statements" />

string[] l = null;
List<string> newL = null;
//var l = File.ReadAllLines(@"C:\Users\matth\AppData\Local\touhou_luna_nights\game0.sav");
//var newL = new List<string>();
//foreach (var line in l)
//{
//	newL.Add(Encoding.UTF8.GetString(Convert.FromBase64String(line)));
//}
//newL.Dump();
//File.WriteAllLines(@"C:\Users\matth\AppData\Local\touhou_luna_nights\game0.txt", newL);

l = File.ReadAllLines(@"C:\Users\matth\AppData\Local\touhou_luna_nights\game2.sav");
newL = new List<string>();
foreach (var line in l)
{
	newL.Add(Encoding.UTF8.GetString(Convert.FromBase64String(line)));
}
File.WriteAllLines(@"C:\Users\matth\AppData\Local\touhou_luna_nights\game2.txt", newL);