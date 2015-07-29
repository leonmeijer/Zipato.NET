using System.Net.Http;
using System.Threading.Tasks;
using LVMS.Zipato.Exceptions;
using LVMS.Zipato.Interfaces;
using LVMS.Zipato.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PortableRest;

namespace LVMS.Zipato
{
    public partial class ZipatoClient : IZipatoClient
    {
        protected string ApiUrl = "https://my.zipato.com/zipato-web/v2/";
        private RestClient _httpClient;
        internal string Jessionid;
        private bool _initialized;
        internal bool UsePollyTransientFaultHandling = true;

        internal const string OnOffCluster = "com.zipato.cluster.OnOff";
        internal const string EndpointTypeActuator = "actuator";
        internal const string ZipaboxInternalName = "Internal device";

        public ZipatoClient()
        {

        }

        public ZipatoClient(bool usePollyTransientFaultHandling)
        {
            UsePollyTransientFaultHandling = usePollyTransientFaultHandling;
        }

        /// <summary>
        /// Initializes a new ZipatoClient instance with a custom API url.
        /// </summary>
        /// <param name="apiUrl">Endpoint address of the Zipato REST API</param>
        public ZipatoClient(string apiUrl) : this()
        {
            ApiUrl = apiUrl;
        }

        public async Task<bool> CheckConnection()
        {
            if (!_initialized)
                return false;
            try
            {
                var zipabox = await GetZipaboxInfo();
                return zipabox.Online;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks whether or not this library and the connection with Zipato is initialized.
        /// </summary>
        public void CheckInitialized()
        {
            if (!_initialized)
                throw new ZipatoException();
        }

        /// <summary>
        /// Authenticate this client with the Zipato API REST service.
        /// </summary>
        /// <param name="userNameEmail">User name (typically the email address you use on my.zipato.com)</param>
        /// <param name="password">Password</param>
        public async Task LoginAsync(string userNameEmail, string password)
        {
            // First call user/init which returns us a nonce
            _httpClient = new RestClient();
            _httpClient.BaseUrl = ApiUrl;
            var jsonSerializerSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            _httpClient.JsonSerializerSettings = jsonSerializerSettings;

            var initRequest = new RestRequest("user/init", HttpMethod.Get);
            
            var initResult = await _httpClient.ExecuteWithPolicyAsync<InitResponse>(this, initRequest);
            if (!initResult.Success)                
                throw new CannotInitializeSessionException();

            // Save the JSessionId, because we pass this as Cookie value to all future requests
            Jessionid = initResult.JSessionId;
            // SHA1-hash the password with the nonce (protects against cross-site forgery)
            string token = Utils.GetToken(password, initResult.Nonce);

            // Sign in with user name and token
            var loginRequest = new RestRequest("user/login", HttpMethod.Get);
            
            loginRequest.AddQueryString("username", userNameEmail);
            loginRequest.AddQueryString("token", token);
            var loginResult = await _httpClient.ExecuteWithPolicyAsync<UserSession>(this, loginRequest);

            if (!loginResult.Success)
                throw new AuthenticationFailureException(loginResult.Error);

            _initialized = true;
        }

        public async Task<Zipabox[]> GetZipaboxesInfo()
        {
            

            var request = new RestRequest("multibox/list", HttpMethod.Get);
            
            return await _httpClient.ExecuteWithPolicyAsync<Zipabox[]>(this, request);
        }

        public async Task<Zipabox> GetZipaboxInfo()
        {
            var boxes = await GetZipaboxesInfo();
            return boxes.Length == 0 ? null : boxes[0];
        }

        //protected virtual void PrepareRequest (RestRequest request)
        //{
            
        //}
    }
}
