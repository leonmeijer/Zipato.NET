using System.Net.Http;
using System.Threading.Tasks;
using LVMS.Zipato.Model;
using PortableRest;

namespace LVMS.Zipato
{
    public partial class ZipatoClient
    {
        /// <summary>
        /// Returns a list of rooms
        /// </summary>
        /// <returns></returns>
        public async Task<Contact[]> GetContactsAsync()
        {
            CheckInitialized();

            // Note that this API call requires the forward slash at the end. This is different than
            // other API calls!
            var request = new RestRequest("contacts", HttpMethod.Get);
            PrepareRequest(request);
            return  await _httpClient.ExecuteAsync<Contact[]>(request);            
        }       
    }
}
