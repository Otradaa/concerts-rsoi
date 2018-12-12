using Gateway.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gateway.Services
{
    public class PerfomersService : IPerfomersService
    {
        //private readonly IOptions<AppSettings> _settings;
        private readonly HttpClient _httpClient;
        //private readonly ILogger<CatalogService> _logger;

        private readonly string _remoteServiceBaseUrl;

        public PerfomersService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            //_settings = settings;
            //_logger = logger;

            _remoteServiceBaseUrl = $"{configuration["PerfomersUrl"]}";
        }
        public async Task<Perfomer> GetById(int id)
        {
            var request = new HttpRequestMessage(new HttpMethod("GET"), 
                _remoteServiceBaseUrl +"/perfomers/" + id.ToString());
            var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadAsAsync<Perfomer>();
        }

        public async Task<List<Perfomer>> GetAll()
        {
            string url = _remoteServiceBaseUrl + $"/perfomers";
            var request = new HttpRequestMessage(new HttpMethod("GET"), url);
            var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadAsAsync<List<Perfomer>>();
        }
    }
}
