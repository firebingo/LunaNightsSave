using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LunaNightsDataWin
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				if (args.Length != 2)
				{
					Console.WriteLine("Invalid Input");
					return;
				}
				var inFile = args[0];
				var outDir = args[1];
				if (!File.Exists(inFile))
				{
					Console.WriteLine("Could not find input file");
					return;
				}
				if (!Directory.Exists(outDir))
					Directory.CreateDirectory(outDir);
				var reader = new LunaDataWinReader(inFile, outDir);
				reader.ReadFiles().Wait();
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}

	public class LunaDataWinReader
	{
		private string _inFile;
		private string _outDir;
		private List<FileDef> _images = null;

		public LunaDataWinReader(string inFile, string outDir)
		{
			_inFile = inFile;
			_outDir = outDir;
			if (File.Exists("AppData\\PngLocations.json"))
				_images = JsonConvert.DeserializeObject<List<FileDef>>(File.ReadAllText("AppData\\PngLocations.json"));
			else
			{
				Console.WriteLine("Finding Pngs in data.win");
				_images = new List<FileDef>();
				FindOffsets();
				Console.WriteLine("Pngs found");
			}
		}

		private void FindOffsets()
		{
			using (BinaryReader reader = new BinaryReader(new FileStream(_inFile, FileMode.Open)))
			{
				var i = 0;
				do
				{
					var foundPng = false;
					long startOffset = -1;
					long endOffset = -1;
					do
					{
						var pos = reader.BaseStream.Position;
						if (pos == reader.BaseStream.Length)
							break;
						var by = reader.ReadByte();
						if (by == 0x89)
						{
							var bytes = reader.ReadBytes(3);
							if (bytes[0] == 0x50 && bytes[1] == 0x4E && bytes[2] == 0x47)
							{
								foundPng = true;
								startOffset = pos;
							}
						}
					} while (!foundPng);

					if (startOffset == -1)
						continue;


					var foundEnd = false;
					do
					{
						var pos = reader.BaseStream.Position;
						if (pos == reader.BaseStream.Length)
							break;
						var by = reader.ReadByte();
						if (by == 0x49)
						{
							var bytes = reader.ReadBytes(4);
							if (bytes[0] == 0x45 && bytes[1] == 0x4E && bytes[2] == 0x44 && bytes[3] == 0xAE)
							{
								foundEnd = true;
								endOffset = pos + 5;
							}
						}
					} while (!foundEnd);

					if (startOffset == -1 || endOffset == -1)
						continue;

					_images.Add(new FileDef(startOffset, endOffset, $"data{i++}"));
				}
				while (reader.BaseStream.Position != reader.BaseStream.Length);

				File.WriteAllText("AppData\\PngLocations.json", JsonConvert.SerializeObject(_images));
			}
		}

		public async Task ReadFiles()
		{
			Console.WriteLine("Extracting Pngs from data.win");
			var tasks = new List<Task>();
			foreach (var d in _images)
			{
				tasks.Add(ReadFile(d));
			}
			await Task.WhenAll(tasks);
			Console.WriteLine("Pngs extracted");
		}

		private async Task ReadFile(FileDef image)
		{
			var outf = $"{_outDir}\\{image.FileName}.png";
			byte[] test = new byte[image.Length];
			using (BinaryReader reader = new BinaryReader(new FileStream(_inFile, FileMode.Open)))
			{
				reader.BaseStream.Seek(image.Start, SeekOrigin.Begin);
				reader.Read(test, 0, (int)image.Length);
			}
			await File.WriteAllBytesAsync(outf, test);
		}
	}

	public class FileDef
	{
		public long Start;
		public long End;
		public long Length;
		public string FileName;

		public FileDef(long start, long end, string fileName)
		{
			Start = start;
			End = end;
			Length = End - Start;
			FileName = fileName;
		}
	}
}
