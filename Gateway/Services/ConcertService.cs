using Gateway.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
    public class ConcertService : IConcertService
    {
       // private readonly IConfiguration _Configuration;
        private readonly HttpClient _httpClient;
        //private readonly ILogger<CatalogService> _logger;

        private readonly string _remoteServiceBaseUrl;

        public ConcertService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            //_Configuration = __Configuration;
            //_logger = logger;
            //httpClient.BaseAddress
            _remoteServiceBaseUrl = $"{configuration["ConcertsUrl"]}";
        }

        public async Task<List<Concert>> GetAll(int page, int size)
        {
            var request = new HttpRequestMessage(new HttpMethod("GET"),
                _remoteServiceBaseUrl+$"/concerts?page={page}&size={size}");
            var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadAsAsync<List<Concert>>();
        }

        public async Task<(bool, Concert)> PostOne(Concert concert)
        {
            var request = new HttpRequestMessage(new HttpMethod("POST"),
                _remoteServiceBaseUrl + "/concerts");
            request.Content = new StringContent(JsonConvert.SerializeObject(concert), 
                Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(request);
            concert = await response.Content.ReadAsAsync<Concert>();
            return (response.IsSuccessStatusCode, concert);
        }

        public async Task<bool> PutOne(int id, Concert concert)
        {
            var request = new HttpRequestMessage(new HttpMethod("PUT"),
                _remoteServiceBaseUrl + "/concerts/" + id.ToString());
            request.Content = new StringContent(JsonConvert.SerializeObject(concert), 
                Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }


    }
}
