using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using trackr.ImageProcessing;

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
            NextSession = DateTime.Now.AddDays(7);
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
            NextSession = DateTime.Now.AddDays(7);
        }
        
        public List<TherapySession> GetSessions() => _sessions;
        
        public TherapySession GetActiveSession()
        {
            if (!_sessions.Any()) return null;
            return _sessions.Last().SessionRunning ? _sessions.Last() : null;
        }


        /// <summary>
        /// Generate one years worth of therapy sessions once or twice a week
        /// This is only for default puroses
        /// </summary>
        /// <param name="startYear">The year for which the data is started to be generated</param>
        /// <param name="howMany">How many years the data is generated for</param>
        public void GenerateTherapySessionData(int startYear, int howMany)
        {
            var now = new DateTime(startYear, 1, 1, 14, 0, 0);
            var rnd = new Random();

            now = now + new TimeSpan(rnd.Next(0,6), 0, 0, 0);

            float angerMean, contemptMean, disgustMean, fearMean, happinessMean, neutralMean, sadnessMean, surpriseMean;

            angerMean = rnd.Next(35, 50);
            contemptMean = rnd.Next(1, 3);
            disgustMean = rnd.Next(1, 10);
            fearMean = rnd.Next(1,10);
            happinessMean = rnd.Next(20,30);
            neutralMean = rnd.Next(30, 45);
            sadnessMean = rnd.Next(35, 50);
            surpriseMean = rnd.Next(1, 5);


            TherapySession session;

            for ( ; ; )
            {
                if (now.Year >= startYear + howMany)
                    return;

                session = new TherapySession(this.Info);
                session.StartDateTime = new DateTime(now.Ticks);

                for (int i = 0; i < 1200; i++)
                {
                    if (angerMean > 0 && rnd.Next(0, (int)(75 * (100 - angerMean))) % 3000 == 0)
                        angerMean--;

                    if (sadnessMean > 0 && rnd.Next(0, (int)(75 * (100 - sadnessMean))) % 3000 == 0)
                        sadnessMean--;

                    if (rnd.Next(0, (int)(60 * happinessMean)) % 3200 == 0)
                        happinessMean++;

                    float anger, contempt, disgust, fear, happiness, neutral, sadness, surprise;

                    anger = rnd.Next((int)angerMean, (int)angerMean+10);
                    contempt = rnd.Next((int)contemptMean, (int)contemptMean+3);
                    disgust = rnd.Next((int)disgustMean, (int)disgustMean+10);
                    fear = rnd.Next((int)fearMean, (int)fearMean+10);
                    happiness = rnd.Next((int)happinessMean, (int)happinessMean+10);
                    neutral = rnd.Next((int)neutralMean, (int)neutralMean+5);
                    sadness = rnd.Next((int)sadnessMean, (int)sadnessMean+10);
                    surprise = rnd.Next((int)surpriseMean, (int)surpriseMean+5);

                    float sum = anger + contempt + disgust + fear + happiness + neutral + sadness + surprise;

                    // Normalize
                    anger /= sum;
                    contempt /= sum;
                    disgust /= sum;
                    fear /= sum;
                    happiness /= sum;
                    neutral /= sum;
                    sadness /= sum;
                    surprise /= sum;

                    var emotionData = new EmotionData(new DateTime(now.Ticks), anger, contempt, disgust, fear, happiness, neutral, sadness, surprise);

                    session.InsertEmotionData(emotionData);

                    now = now + new TimeSpan(0, 0, 3);
                }

                session.EndDateTime = new DateTime(now.Ticks);
                _sessions.Add(session);

                now = now - new TimeSpan(1, 0, 0);

                now = now + new TimeSpan(7, 0, 0, 0);
            }
        }
    }
}