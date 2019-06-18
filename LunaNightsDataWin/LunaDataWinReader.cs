using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LunaNightsDataWin
{
	public class LunaDataWinReader
	{
		private readonly string _outDir;
		private readonly string _inFile;

		//Out directory is optional so you can make a reader just to get the offsets
		public LunaDataWinReader(string inFile, string outDir = null)
		{
			_inFile = inFile;
			_outDir = outDir;
		}

		public ImageFileDef[] FindOffsets()
		{
			var images = new List<ImageFileDef>();
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
							var bytes = reader.ReadBytes(7);
							if (bytes[0] == 0x45 && bytes[1] == 0x4E && bytes[2] == 0x44 && bytes[3] == 0xAE && bytes[4] == 0x42 && bytes[5] == 0x60 && bytes[6] == 0x82)
							{
								foundEnd = true;
								endOffset = pos + 8;
							}
						}
					} while (!foundEnd);

					if (startOffset == -1 || endOffset == -1)
						continue;

					images.Add(new ImageFileDef(startOffset, endOffset, $"data{i++}"));
				}
				while (reader.BaseStream.Position != reader.BaseStream.Length);
			}

			return images.ToArray();
		}

		public async Task ReadFiles(ImageFileDef[] images)
		{
			if (string.IsNullOrWhiteSpace(_outDir))
				throw new Exception("No out directory given");

			var tasks = new List<Task>();
			foreach (var d in images)
			{
				tasks.Add(ReadFile(d));
			}
			await Task.WhenAll(tasks);
		}

		private async Task ReadFile(ImageFileDef image)
		{
			var outf = $"{_outDir}\\{image.FileName}.png";
			byte[] test = new byte[image.Length];
			using (var stream = File.OpenRead(_inFile))
			{
				stream.Seek(image.Start, SeekOrigin.Begin);
				await stream.ReadAsync(test, 0, (int)image.Length);
			}
			using (FileStream stream = File.Open(outf, FileMode.Create))
			{
				await stream.WriteAsync(test, 0, test.Length);
			}
		}
	}
}
