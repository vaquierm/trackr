using System.Configuration;
using System.IO;
using Newtonsoft.Json;

namespace trackr.core
{
    public class TrackrSettings : AppSettings<TrackrSettings>
    {
        public static string DefaultWorkingDir = Directory.GetCurrentDirectory();
        public static string WorkingDirectory;
    }
    
    public class AppSettings<T> where T : new()
    {
        private const string DefaultFilename = "settings.json";

        public void Save(string fileName = DefaultFilename)
        {
            File.WriteAllText(fileName, (JsonConvert.SerializeObject(this)));
        }

        public static T Load(string fileName = DefaultFilename)
        {
            var t = new T();
            if(File.Exists(fileName))
                t = JsonConvert.DeserializeObject<T>(fileName);
            return t;
        }
    } 
}