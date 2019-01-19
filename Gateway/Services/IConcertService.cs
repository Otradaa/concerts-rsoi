using Gateway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gateway.Services
{
    public interface IConcertService
    {
        Task<ConcertsCount> GetAll(int page, int size);
        Task<int> GetCount();
        Task<HttpResponseMessage> PostOne(Concert concert);
        Task<ClientToken> GetToken();
        Task<HttpResponseMessage> PutOne(int id, Concert concert);
    }
}
