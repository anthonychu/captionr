using CaprionR.Mobile.Helpers;
using CaprionR.Mobile.Model;
using Microsoft.AspNetCore.SignalR.Client;
using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CaprionR.Mobile.ViewModel
{
    public class CaptionReaderViewModel : BaseViewModel
    {
        string clientId;
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
            clientId = Guid.NewGuid().ToString();
            fullLanguages = new List<string>();
            Languages = new ObservableRangeCollection<string>();

            GetLanguagesCommand = new Command(async () => await ExecuteGetLanguagesCommand());
            ChangeLanguageCommand = new Command(async () => await ExecuteChangeLanguageCommand());
            hubConnection = new HubConnectionBuilder().WithUrl($"{Constants.ApiBaseUrl}/api/{clientId}").Build();
            hubConnection.Closed += async (error) =>
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
            };


            hubConnection.On<Caption>("newCaption", (caption) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Text = caption.Text;
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
                IsBusy = true;
                var json = await Client.GetStringAsync("api/Languages");
                fullLanguages.Clear();
                fullLanguages.AddRange(JsonConvert.DeserializeObject<IEnumerable<string>>(json));
                Languages.ReplaceRange(fullLanguages.Select(l => l.Substring(0, 2)));
                if(string.IsNullOrWhiteSpace(SelectedLanguage))
                {
                    SelectedLanguage = Languages?.FirstOrDefault() ?? string.Empty;
                }
            }
            catch (Exception)
            {
                Text = "Unable to get languages.";
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task ExecuteChangeLanguageCommand()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                await Client.PutAsync("api/SelectLanguage", new StringContent($""));
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

        public async Task ConnectAsync()
        {
            if (IsConnected)
                return;

            await hubConnection.StartAsync();
            IsConnected = true;
        }

        public async Task DisconnectAsync()
        {
            if (!IsConnected)
                return;

            await hubConnection.StopAsync();

            IsConnected = false;
        }
    }
}
