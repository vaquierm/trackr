using System;
using System.Collections.Generic;
using System.Linq;
using trackr.ImageProcessing;

namespace trackr.core
{
    public class TherapySession
    {
        public TherapyClientInfo TherapyClientInfo { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        private readonly Dictionary<DateTime, string> _notesDictionary;
        private readonly List<EmotionData> _emotionData;
        private bool _sessionRunning;
        
        public TherapySession(TherapyClientInfo clientInfo)
        {
            TherapyClientInfo = clientInfo;
            StartDateTime = DateTime.Now;
            _sessionRunning = true;
            
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
            _sessionRunning = false;
        }
    }
}