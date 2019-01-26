using AuthService.Data;
using AuthService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Gateway.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly string _remoteServiceBaseUrl;

        public AuthService(HttpClient httpClient, IConfiguration configuration)
        {
            //    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.RefreshToken}");
            _remoteServiceBaseUrl = $"{configuration["AuthUrl"]}";

            _httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }
        public async Task<string> LoginApp()//
        {
           // var userContent = new StringContent(JsonConvert.SerializeObject(user), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.GetAsync(_remoteServiceBaseUrl + "/oauth/authorize");
            return response.Headers.Location.AbsoluteUri;
        }

        public async Task<UserTokens> GetTokens(AppUser user)//
        {
            var userContent = new StringContent(JsonConvert.SerializeObject(user), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_remoteServiceBaseUrl + "/oauth/token", userContent);
            return await response.Content.ReadAsAsync<UserTokens>();
        }

        public async Task<HttpResponseMessage> GetAuthCode(User user)//
        {
            var userContent = new StringContent(JsonConvert.SerializeObject(user), System.Text.Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(_remoteServiceBaseUrl + "/oauth/authcode", userContent);
        }

    

        public async Task<UserTokens> Login(User user)
        {
            var userContent = new StringContent(JsonConvert.SerializeObject(user), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_remoteServiceBaseUrl+"/users/login", userContent);
            return await response.Content.ReadAsAsync<UserTokens>();
        }

        public async Task<UserTokens> RefreshTokens(UserTokens token)//
        {
            //   httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.RefreshToken}");
            //   HttpResponseMessage response = await httpClient.GetAsync(uri,);
            var userContent = new StringContent(JsonConvert.SerializeObject(token), System.Text.Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, _remoteServiceBaseUrl + "/oauth/refreshtokens");
            request.Content = userContent;
          //  request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.RefreshToken);
            var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadAsAsync<UserTokens>();
        }

        public async Task<bool> ValidateToken(string accessToken)//
        {
            //   httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.RefreshToken}");
            //   HttpResponseMessage response = await httpClient.GetAsync(uri,);
            var request = new HttpRequestMessage(HttpMethod.Get, _remoteServiceBaseUrl + "/oauth/validate");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", $"{accessToken.Remove(0, 7)}");
            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}
