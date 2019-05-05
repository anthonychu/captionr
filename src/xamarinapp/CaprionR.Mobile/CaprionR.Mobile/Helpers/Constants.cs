using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace CaprionR.Mobile.Helpers
{
    public static class Constants
    {
        static string localhost { get; } = DeviceInfo.Platform == DevicePlatform.Android ? "10.0.2.2" : "localhost";
        public static string ApiBaseUrl => $"https://buildcaptionr.azurewebsites.net";
        //public static string ApiBaseUrl => $"http://{localhost}:7071";
    }
}
