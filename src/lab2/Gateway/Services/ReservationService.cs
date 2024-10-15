using Gateway.DTO;
using Gateway.Models;
using Gateway.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Gateway.Services
{
    public class ReservationService : IReservationService
    {
        private readonly HttpClient _httpClient = new()
        {
            BaseAddress = new Uri("http://reservation:8070/")
        };

        public async Task<PaginationResponse<IEnumerable<Hotels>>?> GetHotelsAsync(int? page,
        int? size)
        {
            using HttpRequestMessage req = new(HttpMethod.Get, $"api/v1/hotels?page={page}&size={size}");
            using var res = await _httpClient.SendAsync(req);
            var response = await res.Content.ReadFromJsonAsync<PaginationResponse<IEnumerable<Hotels>>>();
            return response;
        }

        public async Task<Hotels?> GetHotelsByIdAsync(int? id)
        {
            using HttpRequestMessage req = new(HttpMethod.Get, $"api/v1/hotels/{id}");
            using var res = await _httpClient.SendAsync(req);
            var response = await res.Content.ReadFromJsonAsync<Hotels>();
            return response;
        }

        public async Task<Hotels?> GetHotelsByUidAsync(Guid? id)
        {
            using HttpRequestMessage req = new(HttpMethod.Get, $"api/v1/hotels/byUid");
            req.Content = JsonContent.Create(id, typeof(Guid?));
            using var res = await _httpClient.SendAsync(req);
            var response = await res.Content.ReadFromJsonAsync<Hotels>();
            return response;
        }

        public async Task<IEnumerable<Reservation>?> GetReservationsByUsernameAsync(string username)
        {
            using HttpRequestMessage req = new(HttpMethod.Get, "api/v1/reservations");
            req.Headers.Add("X-User-Name", username);
            using var res = await _httpClient.SendAsync(req);
            var response = await res.Content.ReadFromJsonAsync<IEnumerable<Reservation>>();
            return response;
        }

        public async Task<Reservation?> GetReservationsByUidAsync(Guid reservationUid)
        {
            using HttpRequestMessage req = new(HttpMethod.Get, $"api/v1/reservations/{reservationUid}");
            using var res = await _httpClient.SendAsync(req);
            var response = await res.Content.ReadFromJsonAsync<Reservation>();
            return response;
        }

        public async Task<Reservation?> CreateReservationAsync(string username, Reservation request)
        {
            using HttpRequestMessage req = new(HttpMethod.Post, "api/v1/reservations");
            req.Headers.Add("X-User-Name", username);
            req.Content = JsonContent.Create(request, typeof(Reservation));
            using var res = await _httpClient.SendAsync(req);
            res.EnsureSuccessStatusCode();
            var response = await res.Content.ReadFromJsonAsync<Reservation>();
            return response;
        }

        public async Task<Reservation?> DeleteReservationAsync(Guid reservationUid)
        {
            using HttpRequestMessage req = new(HttpMethod.Delete, $"api/v1/reservations/{reservationUid}");
            using var res = await _httpClient.SendAsync(req);
            var response = await res.Content.ReadFromJsonAsync<Reservation>();
            return response;
        }
    }
}
