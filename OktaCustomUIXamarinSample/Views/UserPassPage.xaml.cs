using OktaCustomUIXamarinSample.Services;
using Xamarin.Forms;

namespace OktaCustomUIXamarinSample.Views
{
    public partial class UserPassPage : ContentPage
    {
        IOktaService _oktaService;

        public UserPassPage()
        {
            InitializeComponent();

            _oktaService = DependencyService.Get<IOktaService>();
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            var idToken = await _oktaService.LogIn(ctlUsername.Text, ctlPassword.Text);
            await Navigation.PushAsync(new LoggedInPage(idToken));
        }

    }
}
