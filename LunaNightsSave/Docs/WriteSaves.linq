<Query Kind="Statements" />

var l = File.ReadAllLines(@"C:\Users\matth\AppData\Local\touhou_luna_nights\game2.txt");
var newL = new List<string>();
foreach (var line in l)
{
	var bytes = Encoding.UTF8.GetBytes(line);
	newL.Add(Convert.ToBase64String(bytes, 0, bytes.Length, Base64FormattingOptions.None));
}
//newL.Dump();
File.WriteAllLines(@"C:\Users\matth\AppData\Local\touhou_luna_nights\game2.sav", newL);