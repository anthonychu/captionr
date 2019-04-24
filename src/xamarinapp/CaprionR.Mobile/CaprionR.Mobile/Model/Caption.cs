using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace CaprionR.Mobile.Model
{
    public class Caption
    {
        [JsonProperty("language")]
        public string Language { get; set; }
        [JsonProperty("offset")]
        public int Offset { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
