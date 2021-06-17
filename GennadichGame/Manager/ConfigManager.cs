using System;
using System.IO;
using Newtonsoft.Json;

namespace GennadichGame.Manager
{
    public class ConfigManager
    {
        #region ConfigFile

        public const string DefaultConfigFilePath = "./";
        public static string ConfigFilePath = DefaultConfigFilePath;
        public const string ConfigFileName = "config.json";

        #endregion

        #region UDP

        public int UdpPort;
        public const int DefaultUdpPort = 11000;
        public int UdpTimeout;
        public const int DefaultUdpTimeout = 3000;
        public string Message;
        public const string DefaultMessage = "GDarts";

        #endregion

        #region TCP

        public int TcpPort;
        public const int DefaultTcpPort = 12345;

        #endregion

        #region PublicMethods

        public ConfigManager()
        {
        }

        public void SaveConfig()
        {
            try
            {
                using var fileStream = new StreamWriter(ConfigFilePath + ConfigFileName);
                var jsonString = JsonConvert.SerializeObject(this);
                fileStream.Write(jsonString);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public static ConfigManager GetConfig()
        {
            try
            {
                using var fileStream = new StreamReader(ConfigFilePath + ConfigFileName);
                var fileData = fileStream.ReadToEnd();
                if (fileData == "")
                    throw new Exception();
                return JsonConvert.DeserializeObject<ConfigManager>(fileData);
            }
            catch (Exception e)
            {
                var config = new ConfigManager()
                {
                    Message = DefaultMessage,
                    TcpPort = DefaultTcpPort,
                    UdpPort = DefaultUdpPort,
                    UdpTimeout = DefaultUdpTimeout
                };
                config.SaveConfig();
                return config;
            }
        }

        #endregion

        #region PrivateMethods

        #endregion
    }
}