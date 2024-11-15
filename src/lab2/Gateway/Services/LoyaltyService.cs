using System.Net.Http;
using System;
using Gateway.Models;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Gateway.Utils;
using System.Net;
using Gateway.ServiceInterfaces;

namespace Gateway.Services
{
    public class LoyaltyService : ILoyaltyService
    {
        private readonly RequestQueueService _requestQueueService;
        private readonly HttpClient _httpClient;
        private readonly CircuitBreaker _circuitBreaker;
        
        public LoyaltyService(RequestQueueService requestQueueService)
        {
            _circuitBreaker = CircuitBreaker.Instance;
            _requestQueueService = new RequestQueueService();
            _requestQueueService.StartWorker();
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://loyalty:8050/");
        }

        public async Task<bool> HealthCheckAsync()
        {
            if (_circuitBreaker.IsOpened())
            {
                return false;
            }
            using var req = new HttpRequestMessage(HttpMethod.Get,
                "manage/health");
            try
            {
                using var res = await _httpClient.SendAsync(req);
                _circuitBreaker.ResetFailureCount();
                return res.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                _circuitBreaker.IncrementFailureCount();
                if (_circuitBreaker.IsOpened())
                {
                    var reqClone = await HttpRequestMessageHelper.CloneHttpRequestMessageAsync(req);
                    _requestQueueService.AddRequestToQueue(reqClone);
                }
                return false;
            }
        }

        public async Task<Loyalty?> GetLoyaltyByUsernameAsync(string username)
        {
            if (_circuitBreaker.IsOpened())
            {
                return null;
            }
            using var req = new HttpRequestMessage(HttpMethod.Get, "api/v1/loyalty");
            req.Headers.Add("X-User-Name", username);
            using var res = await _httpClient.SendAsync(req);
            var response = await res.Content.ReadFromJsonAsync<Loyalty>();
            return response;
        }

        public async Task<Loyalty?> PutLoyaltyByUsernameAsync(string username)
        {
            if (_circuitBreaker.IsOpened())
            {
                return null;
            }
            using var req = new HttpRequestMessage(HttpMethod.Put, "api/v1/loyalty");
            req.Headers.Add("X-User-Name", username);
            using var res = await _httpClient.SendAsync(req);
            var response = await res.Content.ReadFromJsonAsync<Loyalty>();
            return response;
        }

        public async Task<Loyalty?> DeleteLoyaltyByUsernameAsync(string username)
        {
            using var req = new HttpRequestMessage(HttpMethod.Delete, "api/v1/loyalty");
            req.Headers.Add("X-User-Name", username);

            try
            {
                using var res = await _httpClient.SendAsync(req);
                if (!res.IsSuccessStatusCode)
                {
                    var reqClone = await HttpRequestMessageHelper.CloneHttpRequestMessageAsync(req);
                    _requestQueueService.AddRequestToQueue(reqClone);
                    return null;
                }

                var response = await res.Content.ReadFromJsonAsync<Loyalty>();

                return response;
            }
            catch (HttpRequestException)
            {
                var reqClone = await HttpRequestMessageHelper.CloneHttpRequestMessageAsync(req);
                _requestQueueService.AddRequestToQueue(reqClone);
                return null;
            }
        }
    }
}
