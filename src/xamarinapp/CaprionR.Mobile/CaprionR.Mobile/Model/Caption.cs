using System;
using System.Collections.Generic;
using System.Text;
using MvvmHelpers;
using Newtonsoft.Json;

namespace CaprionR.Mobile.Model
{
    public class FullCaption : ObservableObject
    {
        public int Offset { get; set; }
        string text;
        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }
    }
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
