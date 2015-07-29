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
                        

            var request = new RestRequest("security/session/init/", HttpMethod.Get);
            
            return await _httpClient.ExecuteWithPolicyAsync<SecurityResponse>(this, request);           
        }

        public async Task<SecurityResponse> LoginAlarmWithPinAsync(string secureSessionId, string salt, string nonce, string pinCode)
        {
            

            string token = Utils.GetToken(salt + pinCode, nonce);

            var request = new RestRequest("security/session/login/" + secureSessionId + "?token=" + token, HttpMethod.Get);
            
            return await _httpClient.ExecuteWithPolicyAsync<SecurityResponse>(this, request);
        }
    }
}
