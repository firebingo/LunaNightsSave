using LunaNightsSave.Helpers;
using LunaNightsSave.Models;
using Newtonsoft.Json;
using System;
using System.IO;

namespace LunaNightsSave.Function
{
	public class Config
	{
		private static Config _instance;
		public static Config Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new Config();
					_instance.LoadConfig();
				}
				return _instance;
			}
		}

		private readonly string ConfigPath = @"AppData/Config.json";
		public ConfigModel ConfigObject { get; private set; }

		public void LoadConfig()
		{
			try
			{
				var serializerSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };
				if (File.Exists(ConfigPath))
				{
					ConfigObject = JsonConvert.DeserializeObject<ConfigModel>(File.ReadAllText(ConfigPath), serializerSettings);
				}
				else
				{
					ConfigObject = new ConfigModel();
				}
				SaveConfig();
			}
			catch (Exception ex)
			{
				ErrorTracker.CurrentError = ex.Message;
			}
		}

		public void SaveConfig()
		{
			try
			{
				string directoryName = Path.GetDirectoryName(ConfigPath);
				if (!Directory.Exists(directoryName))
					Directory.CreateDirectory(directoryName);
				using (StreamWriter writer = new StreamWriter(File.Create(ConfigPath)))
				{
					JsonSerializer serializer = new JsonSerializer()
					{
						Formatting = Formatting.Indented
					};
					serializer.Serialize(writer, ConfigObject);
				}
			}
			catch (Exception ex)
			{
				ErrorTracker.CurrentError = ex.Message;
			}
		}
	}
}
