using CaprionR.Mobile.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CaprionR.Mobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CaptionReaderView : ContentPage
    {
        CaptionReaderViewModel ViewModel { get; }
        public CaptionReaderView()
        {
            InitializeComponent();
            ViewModel = (CaptionReaderViewModel)BindingContext;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ViewModel.ConnectAsync();
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            await ViewModel.DisconnectAsync();
        }
    }
}