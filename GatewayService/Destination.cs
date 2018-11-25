﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GatewayService
{
    public class Destination
    {
        public string Uri { get; set; }
        public bool RequiresAuthentication { get; set; }
        static HttpClient client = new HttpClient();
        public Destination(string uri, bool requiresAuthentication)
        {
            Uri = uri;
            RequiresAuthentication = requiresAuthentication;
        }

        public Destination(string path)
            : this(path, false)
        {
        }

        private Destination()
        {
            Uri = "/";
            RequiresAuthentication = false;
        }

        public async Task<HttpResponseMessage> SendRequest(HttpRequest request)
        {
            string requestContent;
            using (Stream receiveStream = request.Body)
            {
                using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                {
                    requestContent = readStream.ReadToEnd();
                }
            }

            ///доделать тело реквеста

            var newRequest = new HttpRequestMessage(new HttpMethod(request.Method), CreateDestinationUri(request));

            //newRequest.Content = new StringContent(requestContent, Encoding.UTF8, request.ContentType);
            //request.Path = CreateDestinationUri(request);
            //var response = await client.SendAsync(newRequest);

            return await client.SendAsync(newRequest);


        }

        private string CreateDestinationUri(HttpRequest request)
        {
            string requestPath = request.Path.ToString();
            string queryString = request.QueryString.ToString();

            string endpoint = requestPath.Substring(1).Split('/')[1];

            return Uri + endpoint + queryString;
        }

    }
}
