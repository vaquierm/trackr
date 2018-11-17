using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trackr.ImageProcessing
{
    class EmotionData
    {
        [JsonProperty(PropertyName = "timestamp")]
        public DateTime Timestamp = new DateTime();

        [JsonProperty(PropertyName = "anger")]
        public float Anger { get; set; }

        [JsonProperty(PropertyName = "contempt")]
        public float Contempt { get; set; }

        [JsonProperty(PropertyName = "disgust")]
        public float Disgust { get; set; }

        [JsonProperty(PropertyName = "fear")]
        public float Fear { get; set; }

        [JsonProperty(PropertyName = "hapiness")]
        public float Happiness { get; set; }

        [JsonProperty(PropertyName = "neutral")]
        public float Neutral { get; set; }

        [JsonProperty(PropertyName = "sadness")]
        public float Sadness { get; set; }

        [JsonProperty(PropertyName = "surprise")]
        public float Surprise { get; set; }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static EmotionData Deserialize(string JsonString)
        {
            return JsonConvert.DeserializeObject<EmotionData>(JsonString);
        }
    }
}
