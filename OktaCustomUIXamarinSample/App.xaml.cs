using Xamarin.Forms;
using OktaCustomUIXamarinSample.Services;

namespace OktaCustomUIXamarinSample
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            var oktaService = new OktaService();
            oktaService.ClientId = "????????";
            oktaService.RedirectUri = "com.myappnamespace.exampleapp:/callback";
            oktaService.OktaOrg = "https://dev-??????.okta.com";
            oktaService.AuthorizationServerId = "default";


            DependencyService.RegisterSingleton<IOktaService>(oktaService);
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
