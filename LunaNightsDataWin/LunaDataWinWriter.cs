using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LunaNightsDataWin
{
	public class LunaDataWinWriter
	{
		private readonly string _outFile;

		public LunaDataWinWriter(string outFile)
		{
			_outFile = outFile;
		}

		public async Task WriteFile(ImageFileDef image, string inFile)
		{
			if (!File.Exists(inFile))
				throw new Exception($"{inFile} does not exist");

			var inBytes = File.ReadAllBytes(inFile);
			if (inBytes.Length > image.Length)
				throw new Exception($"{Path.GetFileName(inFile)} is larger than original image");

			byte[] bytes = new byte[image.Length];
			for(var i = 0; i < inBytes.Length; ++i)
			{
				bytes[i] = inBytes[i];
			}

			using (var writer = File.OpenWrite(_outFile))
			{
				writer.Seek(image.Start, SeekOrigin.Begin);
				await writer.WriteAsync(bytes, 0, bytes.Length);
			}
		}
	}
}
