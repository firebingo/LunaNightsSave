using LunaNightsDataWin;
using LunaNightsSave.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LunaNightsSave.Function
{
	public class ImagePacker
	{
		private readonly SynchronizationContext _syncContext;

		public ImagePacker(SynchronizationContext syncContext)
		{
			_syncContext = syncContext;
		}

		private ImageFileDef[] GetOffsets(LunaDataWinReader reader)
		{
			ImageFileDef[] images;
			if (File.Exists("AppData\\PngLocations.json"))
				images = JsonConvert.DeserializeObject<ImageFileDef[]>(File.ReadAllText("AppData\\PngLocations.json"));
			else
			{
				images = reader.FindOffsets();
				File.WriteAllText("AppData\\PngLocations.json", JsonConvert.SerializeObject(images));
			}

			return images;
		}

		public async Task UnpackImagesFromData(string inFile, string outDir)
		{
			try
			{
				_syncContext.Post((s) => ErrorTracker.CurrentError = "Finding Images", null);
				var reader = new LunaDataWinReader(inFile, outDir);
				var images = GetOffsets(reader);
				
				var unpackedMsg = "Images Unpacked";
				_syncContext.Post((s) => ErrorTracker.CurrentError = "Unpacking Images", null);
				await reader.ReadFiles(images);
				_syncContext.Post((s) => ErrorTracker.CurrentError = unpackedMsg, null);
				_ = ErrorTracker.DelayClearError(unpackedMsg, 10000);
			}
			catch (Exception ex)
			{
				_syncContext.Post((s) => ErrorTracker.CurrentError = ex.Message, null);
			}
		}

		public async Task PackImagesIntoData(string[] inFiles, string outFile)
		{
			try
			{
				_syncContext.Post((s) => ErrorTracker.CurrentError = "Finding Images", null);
				var reader = new LunaDataWinReader(outFile);
				var images = GetOffsets(reader);

				_syncContext.Post((s) => ErrorTracker.CurrentError = "Packing Images", null);
				var writer = new LunaDataWinWriter(outFile);
				foreach(var file in inFiles)
				{
					var imageDef = images.FirstOrDefault(x => string.Equals(x.FileName, Path.GetFileNameWithoutExtension(file), StringComparison.InvariantCultureIgnoreCase));
					if (imageDef == null)
					{
						_syncContext.Post((s) => ErrorTracker.CurrentError = $"Could not find image match for {Path.GetFileName(file)}", null);
						return;
					}
					await writer.WriteFile(imageDef, file);
				}
				var packedMsg = "Images Packed";
				_syncContext.Post((s) => ErrorTracker.CurrentError = packedMsg, null);
				_ = ErrorTracker.DelayClearError(packedMsg, 10000);
			}
			catch (Exception ex)
			{
				_syncContext.Post((s) => ErrorTracker.CurrentError = ex.Message, null);
			}
		}
	}
}
