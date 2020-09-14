using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace OktaCustomUIXamarinSample.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new UserPassPage());
        }
    }
}
