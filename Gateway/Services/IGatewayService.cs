using Gateway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gateway.Services
{
    public interface IGatewayService
    {
        Task<ConcertsCount> GetConcerts(int page, int size);
        Task<int> GetConcertsCount();
        Task<List<Perfomer>> GetPerfomers();
        Task<List<Venue>> GetVenues();
        Task<Perfomer> GetPerfomerById(int id);
        Task<Venue> GetVenueById(int id);
        Task<HttpResponseMessage> PostConcert(Concert concert);
        Task<ClientToken> GetToken();
        Task<bool> PostSchedule(Schedule schedule);
        Task<HttpResponseMessage> PutConcert(int id, Concert concert);
        Task<bool> PutSchedule(Schedule schedule);
    }
}
