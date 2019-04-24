using CaprionR.Mobile.Helpers;
using CaprionR.Mobile.Model;
using Microsoft.AspNetCore.SignalR.Client;
using MvvmHelpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CaprionR.Mobile.ViewModel
{
    public class CaptionReaderViewModel : BaseViewModel
    {
        string userId;
        HubConnection hubConnection;
        List<string> fullLanguages;
        public ObservableRangeCollection<string> Languages { get; }
        public ICommand GetLanguagesCommand { get; }
        public ICommand ChangeLanguageCommand { get; }

        bool IsConnected { get; set; }

        HttpClient client;
        HttpClient Client => client ?? (client = new HttpClient()
        {
            BaseAddress = new Uri(Constants.ApiBaseUrl)
        });
        Random random = new Random();
        public CaptionReaderViewModel()
        {
            userId = Guid.NewGuid().ToString();
            fullLanguages = new List<string>();
            Languages = new ObservableRangeCollection<string>();

            GetLanguagesCommand = new Command(async () => await ExecuteGetLanguagesCommand());
            ChangeLanguageCommand = new Command(async () => await ExecuteChangeLanguageCommand());
            var url = $"{Constants.ApiBaseUrl}/api/{userId}";
            hubConnection = new HubConnectionBuilder()
                .WithUrl($"{Constants.ApiBaseUrl}/api/{userId}")
                .Build();
            /*hubConnection.Closed += async (error) =>
            {
                   
                IsConnected = false;
                await Task.Delay(random.Next(0, 5) * 1000);
                try
                {
                    await ConnectAsync();
                }
                catch (Exception ex)
                {
                    // Exception!
                    Debug.WriteLine(ex);
                }
            };*/


            hubConnection.On<JObject>("newCaption", (caption) =>
            {
                var fullCaption = caption.ToObject<Caption>();


                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Text = fullCaption.Text;
                });
            });

        }

        string text = string.Empty;
        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        string selectedLanguage = string.Empty;
        public string SelectedLanguage
        {
            get => selectedLanguage;
            set
            {
                if (SetProperty(ref selectedLanguage, value))
                    ChangeLanguageCommand.Execute(null);
            }
        }

        async Task ExecuteGetLanguagesCommand()
        {
            if (IsBusy)
                return;
            try
            {
                Title = "Getting Languages...";
                IsBusy = true;
                var json = await Client.GetStringAsync("api/Languages");
                fullLanguages.Clear();
                fullLanguages.AddRange(JsonConvert.DeserializeObject<IEnumerable<string>>(json));
                Languages.ReplaceRange(fullLanguages.Select(l => l.Substring(0, 2)));
                

                Title = "Receiving...";
            }
            catch (Exception)
            {
                Text = "Unable to get languages.";
            }
            finally
            {
                IsBusy = false;
            }

            if (string.IsNullOrWhiteSpace(SelectedLanguage))
            {
                SelectedLanguage = Languages?.FirstOrDefault() ?? string.Empty;
            }
        }

        async Task ExecuteChangeLanguageCommand()
        {
            if (IsBusy || !IsConnected)
                return;
            try
            {
                Title = "Setting Language...";
                IsBusy = true;

                var selected = new SelectLanguage
                {
                    LanguageCode = SelectedLanguage,
                    UserId = this.userId
                };
                var byteContent = CreatePostContent(selected);
                await Client.PostAsync("api/SelectLanguage", byteContent);
                Title = "Receiving...";
            }
            catch (Exception)
            {
                Text = "Unable to select language.";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private static ByteArrayContent CreatePostContent(object item)
        {
            var json = JsonConvert.SerializeObject(item);
            var buffer = Encoding.UTF8.GetBytes(json);
            var byteContent = new ByteArrayContent(buffer);

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return byteContent;
        }

        public async Task ConnectAsync()
        {
            if (IsConnected)
                return;

            Title = "Connecting...";
            await hubConnection.StartAsync();
            IsConnected = true;
            Title = "Connected...";
        }

        public async Task DisconnectAsync()
        {
            if (!IsConnected)
                return;
            Title = "Disconnected...";
            await hubConnection.StopAsync();
            Title = "Disconnected...";
            IsConnected = false;
        }
    }
}
