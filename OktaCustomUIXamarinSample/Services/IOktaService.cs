using System.Threading.Tasks;

namespace OktaCustomUIXamarinSample.Services
{
    public interface IOktaService
    {
        Task<string> LogIn(string username, string password);
        
        string OktaOrg { get; set; }
        string AuthorizationServerId { get; set; }
        string CodeVerifier { get; }
        string ClientId { get; set; }
        string RedirectUri { get; set; }        
    }
}
