using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using trackr.ImageProcessing;

namespace trackr.core
{
    public class TherapySession
    {
        [JsonProperty(PropertyName = nameof(TherapyClientInfo))]
        public TherapyPatientInfo TherapyClientInfo { get; set; }
        
        [JsonProperty(PropertyName = nameof(StartDateTime))]
        public DateTime StartDateTime { get; set; }
        
        [JsonProperty(PropertyName = nameof(EndDateTime))]
        public DateTime EndDateTime { get; set; }
        
        public bool SessionRunning { get; set; }

        [JsonProperty(PropertyName = nameof(_notesDictionary), Required = Required.Always)]
        private readonly Dictionary<DateTime, string> _notesDictionary;
        
        [JsonProperty(PropertyName = nameof(_emotionData), Required = Required.Always)]
        private readonly List<EmotionData> _emotionData;
        
        public TherapySession(TherapyPatientInfo clientInfo)
        {
            TherapyClientInfo = clientInfo;
            StartDateTime = DateTime.Now;
            SessionRunning = true;
            
            _notesDictionary = new Dictionary<DateTime, string>();
            _emotionData = new List<EmotionData>();
        }

        public void InsertNote(DateTime timeStamp, string content)
        {
            _notesDictionary.Add(timeStamp, content);
        }
        
        public void InsertEmotionData(EmotionData data)
        {
            _emotionData.Add(data);
        }

        public KeyValuePair<DateTime, string> FindClosestNote(DateTime timeStamp)
        {
            var ret = new KeyValuePair<DateTime, string>();
            var found = _notesDictionary.OrderBy(x => Math.Abs(timeStamp.Millisecond - x.Key.Millisecond)).First();
            return ret;
        }

        public void EndSession()
        {
            EndDateTime = DateTime.Now;
            SessionRunning = false;
        }

        public Dictionary<DateTime, string> GetNotesDictionary()
        {
            return _notesDictionary;
        }
        
        public List<EmotionData> GetEmotionDataList()
        {
            return _emotionData;
        }
    }
}