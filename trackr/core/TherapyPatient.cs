using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace trackr.core
{
    public class TherapyPatient
    {
        public TherapyPatientInfo Info { get; set; }
        public string PatientStringId => Info.Name + "_" + Info.LastName;

        [JsonProperty(PropertyName = "sessions")]
        private readonly List<TherapySession> _sessions;

        [JsonProperty(PropertyName = nameof(NextSession))]
        public DateTime NextSession { get; set; }
        
        public TherapyPatient(string name, string lastName, Gender gender, DateTime birthDate)
        {
            Info = new TherapyPatientInfo {Name = name, LastName = lastName, Gender = gender, BirthDate = birthDate};
            _sessions = new List<TherapySession>();
        }

        public TherapySession NewSession()
        {
            if (_sessions.Any() && _sessions.Last().SessionRunning)
            {
                _sessions.Last().EndSession();
            }
            _sessions.Add(new TherapySession(Info));
            return _sessions.Last();
        }

        public void EndSession()
        {
            GetActiveSession()?.EndSession();
        }
        
        public List<TherapySession> GetSessions() => _sessions;
        
        public TherapySession GetActiveSession()
        {
            if (!_sessions.Any()) return null;
            return _sessions.Last().SessionRunning ? _sessions.Last() : null;
        }
    }
}