using AuthService.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
        public async Task<UsersToken> Login(User user)
        {
            var userContent = new StringContent(JsonConvert.SerializeObject(user), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_remoteServiceBaseUrl+"/users", userContent);
            return await response.Content.ReadAsAsync<UsersToken>();
            /*try
            {
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var usersToken = JsonConvert.DeserializeObject<UsersToken>(responseBody);

                return usersToken;
            }
            catch (HttpRequestException e)
            {
                if (response.StatusCode != System.Net.HttpStatusCode.Unauthorized)
                    throw e;
            }
            catch (Exception e)
            {
                throw e;
            }

            return null;*/
        }
        public async Task<UsersToken> RefreshTokens(UsersToken token)
        {
            //   httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.RefreshToken}");
            //   HttpResponseMessage response = await httpClient.GetAsync(uri,);
            var request = new HttpRequestMessage(HttpMethod.Get, _remoteServiceBaseUrl + "/refreshtokens");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.RefreshToken);
            var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadAsAsync<UsersToken>();
            /*try
            {
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var usersToken = JsonConvert.DeserializeObject<UsersToken>(responseBody);

                return usersToken;
            }
            catch (HttpRequestException e)
            {
                if (response.StatusCode != System.Net.HttpStatusCode.Unauthorized)
                    throw e;
            }
            catch (Exception e)
            {
                throw e;
            }

            return null;*/

        }

        public async Task<bool> ValidateToken(string accessToken)
        {
            //   httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.RefreshToken}");
            //   HttpResponseMessage response = await httpClient.GetAsync(uri,);
            var request = new HttpRequestMessage(HttpMethod.Get, _remoteServiceBaseUrl + "/validate");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", $"{accessToken.Remove(0, 7)}");
            var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadAsAsync<bool>();

            /*
            try
            {
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (HttpRequestException e)
            {
                if (response.StatusCode != System.Net.HttpStatusCode.Unauthorized)
                    throw e;
            }
            catch (Exception e)
            {
                throw e;
            }

            return false;*/

        }
    }
}
