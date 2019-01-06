using PerfomersServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerfomersServer.Data
{
    public interface IRepository
    {
        Task<Perfomer> GetPerfomer(int id);
        IEnumerable<Perfomer> GetAllPerfomers();
    }
}
