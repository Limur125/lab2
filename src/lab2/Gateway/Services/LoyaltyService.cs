using Gateway.Models;
using Gateway.ServiceInterfaces;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Gateway.Services
{
    public class LoyaltyService : ILoyaltyService
    {
        private readonly HttpClient _httpClient = new()
        {
            BaseAddress = new Uri("http://loyalty:8050/")
        };

        public async Task<Loyalty?> GetLoyaltyByUsernameAsync(string username)
        {
            using HttpRequestMessage req = new(HttpMethod.Get, "api/v1/loyalty");
            req.Headers.Add("X-User-Name", username);
            using var res = await _httpClient.SendAsync(req);
            var response = await res.Content.ReadFromJsonAsync<Loyalty>();
            return response;
        }

        public async Task<Loyalty?> PutLoyaltyByUsernameAsync(string username)
        {
            using HttpRequestMessage req = new(HttpMethod.Put, "api/v1/loyalty");
            req.Headers.Add("X-User-Name", username);
            using var res = await _httpClient.SendAsync(req);
            var response = await res.Content.ReadFromJsonAsync<Loyalty>();
            return response;
        }

        public async Task<Loyalty?> DeleteLoyaltyByUsernameAsync(string username)
        {
            using HttpRequestMessage req = new(HttpMethod.Delete, "api/v1/loyalty");
            req.Headers.Add("X-User-Name", username);
            using var res = await _httpClient.SendAsync(req);
            var response = await res.Content.ReadFromJsonAsync<Loyalty>();
            return response;
        }
    }
}
