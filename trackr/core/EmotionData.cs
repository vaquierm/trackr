using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trackr.ImageProcessing
{
    public class EmotionData
    {
        [JsonProperty(PropertyName = "timestamp")]
        public DateTime TimeStamp = DateTime.Now;

        [JsonProperty(PropertyName = "anger")]
        public float Anger { get; set; }

        [JsonProperty(PropertyName = "contempt")]
        public float Contempt { get; set; }

        [JsonProperty(PropertyName = "disgust")]
        public float Disgust { get; set; }

        [JsonProperty(PropertyName = "fear")]
        public float Fear { get; set; }

        [JsonProperty(PropertyName = "happiness")]
        public float Happiness { get; set; }

        [JsonProperty(PropertyName = "neutral")]
        public float Neutral { get; set; }

        [JsonProperty(PropertyName = "sadness")]
        public float Sadness { get; set; }

        [JsonProperty(PropertyName = "surprise")]
        public float Surprise { get; set; }

        public EmotionData(DateTime date, float anger, float contempt, float disgust, float fear, float happiness, float neutral, float sadness, float surprise)
        {
            this.TimeStamp = date;
            this.Anger = anger;
            this.Contempt = contempt;
            this.Disgust = disgust;
            this.Happiness = happiness;
            this.Neutral = neutral;
            this.Sadness = sadness;
            this.Surprise = surprise;
        }

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
