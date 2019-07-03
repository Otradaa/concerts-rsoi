using Gateway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Services
{
    public interface IPerfomersService
    {
        Task<Perfomer> GetById(int id);
        Task<List<Perfomer>> GetAll();
    }
}
