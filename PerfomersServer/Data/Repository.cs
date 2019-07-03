using PerfomersServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerfomersServer.Data
{
    public class Repository : IRepository
    {
        private readonly PerfomersServerContext _context;

        public Repository(PerfomersServerContext context)
        {
            _context = context; 
        }
        /*public Repository(PerfomersServerContext context)
        {
            _context = context;
        }*/

        public IEnumerable<Perfomer> GetAllPerfomers()
        {
            return _context.Perfomer;
        }

        public async Task<Perfomer> GetPerfomer(int id)
        {
            return await _context.Perfomer.FindAsync(id);
        }
    }
}
