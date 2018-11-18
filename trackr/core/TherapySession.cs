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

        [JsonProperty(PropertyName = nameof(_notesList), Required = Required.Always)]
        private readonly List<Note> _notesList;
        
        [JsonProperty(PropertyName = nameof(_emotionData), Required = Required.Always)]
        private readonly List<EmotionData> _emotionData;
        
        public TherapySession(TherapyPatientInfo clientInfo)
        {
            TherapyClientInfo = clientInfo;
            StartDateTime = DateTime.Now;
            SessionRunning = true;
            
            _notesList = new List<Note>();
            _emotionData = new List<EmotionData>();
        }

        public void InsertNote(DateTime timeStamp, string content)
        {
            _notesList.Add(new Note {TimeStamp = timeStamp, Content = content});
        }
        
        public void InsertEmotionData(EmotionData data)
        {
            _emotionData.Add(data);
        }

        public Note FindClosestNote(DateTime timeStamp)
        {
            var found = _notesList.OrderBy(x => Math.Abs(timeStamp.Millisecond - x.TimeStamp.Millisecond)).First();
            return found;
        }

        public void EndSession()
        {
            EndDateTime = DateTime.Now;
            SessionRunning = false;
        }

        public List<Note> GetNotesList()
        {
            return _notesList;
        }
        
        public List<EmotionData> GetEmotionDataList()
        {
            return _emotionData;
        }
        
        public override string ToString()
        {
            return StartDateTime + " - " + EndDateTime;
        }
    }
}