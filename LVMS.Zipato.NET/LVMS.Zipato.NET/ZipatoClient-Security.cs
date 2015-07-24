using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LVMS.Zipato.Model;
using PortableRest;

namespace LVMS.Zipato
{
    public partial class ZipatoClient
    {
       public async Task<SecurityResponse> InitializeSecuritySessionAsync()
        {
            CheckInitialized();            

            var request = new RestRequest("security/session/init/", HttpMethod.Get);
            PrepareRequest(request);
            return await _httpClient.ExecuteAsync<SecurityResponse>(request);           
        }

        public async Task<SecurityResponse> LoginAlarmWithPinAsync(string secureSessionId, string salt, string nonce, string pinCode)
        {
            CheckInitialized();

            string token = Utils.GetToken(salt + pinCode, nonce);

            var request = new RestRequest("security/session/login/" + secureSessionId + "?token=" + token, HttpMethod.Get);
            PrepareRequest(request);
            return await _httpClient.ExecuteAsync<SecurityResponse>(request);
        }
    }
}
