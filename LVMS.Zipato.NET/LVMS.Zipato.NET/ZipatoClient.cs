using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LVMS.Zipato.Exceptions;
using LVMS.Zipato.Interfaces;
using LVMS.Zipato.Model;
using PortableRest;

namespace LVMS.Zipato
{
    public partial class ZipatoClient : IZipatoClient
    {
        protected string ApiUrl = "https://my.zipato.com/zipato-web/v2/";
        private RestClient _httpClient;
        private string _jessionid;
        private bool _initialized;

        public ZipatoClient()
        {

        }

        /// <summary>
        /// Initializes a new ZipatoClient instance with a custom API url.
        /// </summary>
        /// <param name="apiUrl">Endpoint address of the Zipato REST API</param>
        public ZipatoClient(string apiUrl) : this()
        {
            ApiUrl = apiUrl;
        }

        public Task<bool> CheckConnection()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> LoginAsync(string userNameEmail, string password)
        {
            // First call user/init which returns us a nonce
            _httpClient = new RestClient();
            _httpClient.BaseUrl = ApiUrl;
            
            var initRequest = new RestRequest("user/init", HttpMethod.Get);
            
            var initResult = await _httpClient.ExecuteAsync<InitResponse>(initRequest);
            if (!initResult.Success)                
                throw new CannotInitializeSessionException();

            // Save the JSessionId, because we pass this as Cookie value to all future requests
            _jessionid = initResult.JSessionId;
            // SHA1-hash the password with the nonce (protects against cross-site forgery)
            string token = Utils.GetToken(password, initResult.Nonce);

            // Sign in with user name and token
            var loginRequest = new RestRequest("user/login", HttpMethod.Get);
            
            loginRequest.AddQueryString("username", userNameEmail);
            loginRequest.AddQueryString("token", token);
            PrepareRequest(loginRequest);
            var loginResult = await _httpClient.ExecuteAsync<UserSession>(loginRequest);

            if (!loginResult.Success)
                throw new AuthenticationFailureException(loginResult.Error);

            _initialized = true;

            return true;
        }

        public void CheckInitialized()
        {
            if (!_initialized)
                throw new ZipatoException();
        }

        protected virtual void PrepareRequest (RestRequest request)
        {
            request.AddHeader("Cookie", "JSESSIONID=" + _jessionid);
        }
    }
}
