using Gateway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Services
{
    public interface IGatewayService
    {
        Task<List<Concert>> GetConcerts(int page, int size);
        Task<Perfomer> GetPerfomerById(int id);
        Task<Venue> GetVenueById(int id);
        Task<(bool, int)> PostConcert(Concert concert);
        Task<bool> PostSchedule(Schedule schedule);
        Task<bool> PutConcert(int id, Concert concert);
        Task<bool> PutSchedule(Schedule schedule);
    }
}
