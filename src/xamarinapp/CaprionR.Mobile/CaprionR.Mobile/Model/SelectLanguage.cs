using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CaprionR.Mobile.Model
{
    public class SelectLanguage
    {
        [JsonProperty("languageCode")]
        public string LanguageCode { get; set; }
        
        [JsonProperty("userId")]
        public string UserId { get; set; }
    }
}
