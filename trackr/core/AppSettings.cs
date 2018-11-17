using System.Configuration;
using System.IO;
using Newtonsoft.Json;

namespace trackr.core
{
    public class TrackrSettings : AppSettings<TrackrSettings>
    {
        [JsonProperty(PropertyName = nameof(UseDefaultDir))]
        public bool UseDefaultDir = true;
        
        [JsonProperty(PropertyName = nameof(DefaultWorkingDir))]
        public string DefaultWorkingDir = Path.Combine(Directory.GetCurrentDirectory(), "TrackrWorkspace");
        
        [JsonProperty(PropertyName = nameof(WorkingDirectory))]
        public string WorkingDirectory = string.Empty;
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
            if (File.Exists(fileName))
            {
                t = JsonConvert.DeserializeObject<T>(fileName);
            }
            return t;
        }
    } 
}