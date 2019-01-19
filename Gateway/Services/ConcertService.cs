using Gateway.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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

        public async Task<int> GetCount()
        {
            string url = _remoteServiceBaseUrl + $"/concerts/count";
            var request = new HttpRequestMessage(new HttpMethod("GET"), url);
            var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadAsAsync<int>();
        }

        public async Task<ConcertsCount> GetAll(int page, int size)
        {
            string url = _remoteServiceBaseUrl + $"/concerts";
            if (page > 0 && size > 0)
                url += $"?page={page}&size={size}";
            var request = new HttpRequestMessage(new HttpMethod("GET"), url);
            var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadAsAsync<ConcertsCount>();
        }

        public async Task<ClientToken> GetToken()
        {
            var request = new HttpRequestMessage(new HttpMethod("POST"),
            _remoteServiceBaseUrl + "/auth");
            var secrets = new Client() { appKey = Secrets.appKey, appSecret = Secrets.appSecret };
            request.Content = new StringContent(JsonConvert.SerializeObject(secrets),
                Encoding.UTF8, "application/json");

            var responce = await _httpClient.SendAsync(request);
            return await responce.Content.ReadAsAsync<ClientToken>();
        }
        public async Task<HttpResponseMessage> PostOne(Concert concert, ClientToken token)
        {
            var request = new HttpRequestMessage(new HttpMethod("POST"),
                _remoteServiceBaseUrl + "/concerts");
            if (token.Token != null)
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
            request.Content = new StringContent(JsonConvert.SerializeObject(concert), 
                Encoding.UTF8, "application/json");

            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> PutOne(int id, Concert concert)
        {
            var request = new HttpRequestMessage(new HttpMethod("PUT"),
                _remoteServiceBaseUrl + "/concerts/" + id.ToString());
            request.Content = new StringContent(JsonConvert.SerializeObject(concert), 
                Encoding.UTF8, "application/json");
            return await _httpClient.SendAsync(request);
        }


    }
}
