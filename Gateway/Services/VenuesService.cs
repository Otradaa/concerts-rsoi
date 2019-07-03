using Gateway.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Services
{
    public class VenuesService : IVenuesService
    {
        //private readonly IOptions<AppSettings> _settings;
        private readonly HttpClient _httpClient;
        //private readonly ILogger<CatalogService> _logger;

        private readonly string _remoteServiceBaseUrl;

        public VenuesService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            //_settings = settings;
            //_logger = logger;

            _remoteServiceBaseUrl = $"{configuration["VenuesUrl"]}";
        }
        public async Task<Venue> GetById(int id)
        {
            var request = new HttpRequestMessage(new HttpMethod("GET"),
                _remoteServiceBaseUrl + "/venues/" + id.ToString());
            var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadAsAsync<Venue>();
        }

        public async Task<List<Venue>> GetAll()
        {
            string url = _remoteServiceBaseUrl + $"/venues";
            
            var request = new HttpRequestMessage(new HttpMethod("GET"), url);
            var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadAsAsync<List<Venue>>();
        }

        public async Task<bool> PostSchedule(Schedule schedule)
        {
            var request = new HttpRequestMessage(new HttpMethod("POST"),
                _remoteServiceBaseUrl + "/schedules");
            request.Content = new StringContent(JsonConvert.SerializeObject(schedule), 
                Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> PutSchedule(Schedule schedule)
        {
            var request = new HttpRequestMessage(new HttpMethod("PUT"),
                _remoteServiceBaseUrl + "/schedules");
            request.Content = new StringContent(JsonConvert.SerializeObject(schedule), 
                Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}
