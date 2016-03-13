using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LVMS.Zipato.Exceptions;
using Newtonsoft.Json;
using Polly;
using PortableRest;

namespace LVMS.Zipato
{
    internal static class TransientExceptionHandler
    {
        internal static async Task<T> ExecuteWithPolicyAsync<T>(this RestClient restClient, ZipatoClient zipatoClient, RestRequest restRequest,
            CancellationToken cancellationToken = default(CancellationToken), bool retryHttpPostAndPut = true, bool byPassCheckInitialized = false) where T : class
        {
            PreSendRequest<T>(zipatoClient, restRequest, byPassCheckInitialized);

            if (!zipatoClient.UsePollyTransientFaultHandling)
                return await restClient.ExecuteAsync<T>(restRequest, cancellationToken);

            try
            {
                var retVal = await SendRequestWithPolicy<T>(restClient, restRequest, cancellationToken);
                return retVal.Content;
            }
            catch (Exception e)
            {
                throw new RequestFailedException("Even with transient fault handling enabled, the request failed.", e);
            }
        }

        internal static async Task<RestResponse<T>> SendWithPolicyAsync<T>(this RestClient restClient, ZipatoClient zipatoClient, RestRequest restRequest,
            CancellationToken cancellationToken = default(CancellationToken), bool retryHttpPostAndPut = true, bool byPassCheckInitialized = false) where T : class
        {
            PreSendRequest<T>(zipatoClient, restRequest, byPassCheckInitialized);

            if (!zipatoClient.UsePollyTransientFaultHandling)
                return await restClient.SendAsync<T>(restRequest, cancellationToken);

            try
            {
                var retVal = await SendRequestWithPolicy<T>(restClient, restRequest, cancellationToken);
                return retVal;
            }
            catch (Exception e)
            {
                throw new RequestFailedException("Even with transient fault handling enabled, the request failed.", e);
            }
        }

        private static async Task<RestResponse<T>> SendRequestWithPolicy<T>(RestClient restClient, RestRequest restRequest, CancellationToken cancellationToken)
            where T : class
        {
            var retVal = await Policy
                .Handle<JsonReaderException>()
                .Or<RequestFailedException>()
                .Or<TaskCanceledException>()
                .Or<HttpRequestException>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                .ExecuteAsync<RestResponse<T>>(
                    () => SendRequestAndCheckForException<T>(restClient, restRequest, cancellationToken));
            return retVal;
        }

        private static void PreSendRequest<T>(ZipatoClient zipatoClient, RestRequest restRequest, bool byPassCheckInitialized) where T : class
        {
            if (zipatoClient == null)
                throw new ArgumentNullException("zipatoClient");

            if (!byPassCheckInitialized)
                zipatoClient.CheckInitialized();
            restRequest.AddHeader("Cookie", "JSESSIONID=" + zipatoClient.JSessionId);
        }

        private static async Task<RestResponse<T>> SendRequestAndCheckForException<T>(RestClient restClient, RestRequest restRequest,
            CancellationToken cancellationToken) where T : class
        {
            var response = await restClient.SendAsync<T>(restRequest, cancellationToken);
            if (response.Exception != null)
                throw response.Exception;
            else if (response.HttpResponseMessage.IsSuccessStatusCode)
                return response;
            else if (response.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound ||
                     response.HttpResponseMessage.StatusCode == HttpStatusCode.Forbidden ||
                     response.HttpResponseMessage.StatusCode == HttpStatusCode.InternalServerError ||
                     response.HttpResponseMessage.StatusCode == HttpStatusCode.ServiceUnavailable ||
                     response.HttpResponseMessage.StatusCode == HttpStatusCode.BadGateway ||
                     response.HttpResponseMessage.StatusCode == HttpStatusCode.GatewayTimeout ||
                     response.HttpResponseMessage.StatusCode == HttpStatusCode.NoContent ||
                     response.HttpResponseMessage.StatusCode == HttpStatusCode.RequestTimeout)
                throw new RequestFailedException(response.HttpResponseMessage.StatusCode);
            else
                throw new ZipatoException("Unexpected HTTP status code. Code: " + response.HttpResponseMessage.StatusCode);
        }
    }
}
