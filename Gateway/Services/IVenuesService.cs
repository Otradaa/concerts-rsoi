using Gateway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gateway.Services
{
    public interface IVenuesService
    {
        Task<Venue> GetById(int id);
        Task<List<Venue>> GetAll();
        Task<HttpResponseMessage> PostSchedule(Schedule schedule);
        Task<bool> PutSchedule(Schedule schedule);
    }
}
