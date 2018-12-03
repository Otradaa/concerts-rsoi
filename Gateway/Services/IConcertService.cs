using Gateway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Services
{
    public interface IConcertService
    {
        Task<List<Concert>> GetAll(int page, int size);
        Task<(bool, Concert)> PostOne(Concert concert);
        Task<bool> PutOne(int id, Concert concert);
    }
}
