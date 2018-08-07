using System;
using System.IO;

namespace Cherry.Data.Configuration
{
    public static class Key
    {
        public static string ConfigPath = "database.key";
        
        public static string LoadKey(out string key)
        {
            if (!File.Exists(ConfigPath)) throw new Exception("Key file does not exists or wrong path has been provided");
            try
            {
                key = File.ReadAllText(ConfigPath);
                if (key.Length != 64)
                {
                    throw new Exception();
                }
                return key;
                
            }
            catch
            {
                throw new Exception("Key file is corrupted");
            }
        }
    }
}
