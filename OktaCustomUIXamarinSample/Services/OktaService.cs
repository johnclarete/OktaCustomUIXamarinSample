using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OktaCustomUIXamarinSample.Models;

namespace OktaCustomUIXamarinSample.Services
{
    public class OktaService : IOktaService
    {
        private readonly HttpClient _client;

        public OktaService() 
        {
            _client = new HttpClient(new HttpClientHandler { AllowAutoRedirect = false });
        }

        public string OktaOrg { get; set; }
        public string ClientId { get; set; }
        public string RedirectUri { get; set; }
        public string CodeVerifier { get; private set; }
        public string AuthorizationServerId { get; set; }
        
        public async Task<string> LogIn(string username, string password)
        {
            var sessionToken = await GetSessionToken(username, password);
            var authorizationCode = await StartAuthorizationFlow(GetAuthorizationUrl(sessionToken));
            var idToken = await CompleteAuthorizationFlow(authorizationCode);
            return idToken;
        }

        private async Task<string> GetSessionToken(string username, string password)
        {
            var oktaAuthnEndpoint = $"{this.OktaOrg}/api/v1/authn";
            var json = $"{{\"username\": \"{username}\",\"password\": \"{password}\"}}";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(oktaAuthnEndpoint, content);
            var jsonData = await response.Content.ReadAsStringAsync();
            var authnResponse = JsonConvert.DeserializeObject<AuthnResponse>(jsonData);

            return authnResponse.SessionToken;            
        }

        private async Task<string> StartAuthorizationFlow(string authorizeUri)
        {
            var response = await _client.GetAsync(authorizeUri);
            if (response.StatusCode == System.Net.HttpStatusCode.Redirect)
            {
                var queryString = response.Headers.Location.Query;
                var queryItems = System.Web.HttpUtility.ParseQueryString(queryString);
                return queryItems["code"];
            }
            return string.Empty;
        }

        private string GetAuthorizationUrl(string sessionToken)
        {
            var state = Guid.NewGuid();
            var nonce = Guid.NewGuid();
            GenerateStateCodeVerifierAndChallenge(out string codeVerifier, out string codeChallenge);
            this.CodeVerifier = codeVerifier;
            var url = $"{this.OktaOrg}/oauth2/{this.AuthorizationServerId}/v1/authorize?client_id={this.ClientId}&response_type=code&response_mode=query&scope=openid&redirect_uri={this.RedirectUri}&state={state}&nonce={nonce}&code_challenge_method=S256&code_challenge={codeChallenge}&sessionToken={sessionToken}";
            return url;
        }

        private void GenerateStateCodeVerifierAndChallenge(out string codeVerifier, out string codeChallenge)
        {
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] tokenData = new byte[64];
                rng.GetBytes(tokenData);
                codeVerifier = Microsoft.IdentityModel.Tokens.Base64UrlEncoder.Encode(tokenData);
            }

            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));
                codeChallenge = Microsoft.IdentityModel.Tokens.Base64UrlEncoder.Encode(bytes);
            }
        }

        private async Task<string> CompleteAuthorizationFlow(string authorizationCode)
        {
            var oktaTokenEndpoint = $"{this.OktaOrg}/oauth2/{this.AuthorizationServerId}/v1/token";
            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("grant_type", "authorization_code"));
            keyValues.Add(new KeyValuePair<string, string>("client_id", this.ClientId));
            keyValues.Add(new KeyValuePair<string, string>("redirect_uri", this.RedirectUri));
            keyValues.Add(new KeyValuePair<string, string>("code", authorizationCode));
            keyValues.Add(new KeyValuePair<string, string>("code_verifier", this.CodeVerifier));
            var content = new FormUrlEncodedContent(keyValues);

            var response = await _client.PostAsync(oktaTokenEndpoint, content);
            var jsonData = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(jsonData);

            return tokenResponse.IdToken;
        }
    }
}
