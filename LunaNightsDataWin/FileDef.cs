using System;
using System.Collections.Generic;
using System.Text;

namespace LunaNightsDataWin
{
	public class ImageFileDef
	{
		public readonly long Start;
		public readonly long End;
		public readonly long Length;
		public readonly string FileName;

		public ImageFileDef(long start, long end, string fileName)
		{
			Start = start;
			End = end;
			Length = End - Start;
			FileName = fileName;
		}
	}
}
