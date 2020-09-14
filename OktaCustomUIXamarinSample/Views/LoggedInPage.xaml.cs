using Xamarin.Forms;

namespace OktaCustomUIXamarinSample.Views
{
    public partial class LoggedInPage : ContentPage
    {
        public LoggedInPage(string idToken)
        {
            InitializeComponent();
            lblIdToken.Text = idToken;
        }
    }
}
