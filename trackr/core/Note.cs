using System;
using Newtonsoft.Json;

namespace trackr.core
{
    public struct Note
    {
        [JsonProperty(PropertyName = nameof(TimeStamp))]
        public DateTime TimeStamp;
        
        [JsonProperty(PropertyName = nameof(Content))]
        public string Content;

        public override string ToString()
        {
            return "[" + TimeStamp + "] " + Content;
        }
    }
}