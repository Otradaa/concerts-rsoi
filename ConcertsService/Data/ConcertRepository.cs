using ConcertsService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConcertsService.Data
{
    public class ConcertRepository : IConcertRepository
    {
        private readonly ConcertsContext _context;

        public ConcertRepository(ConcertsContext context)
        {
            _context = context;
        }

        public IEnumerable<Concert> GetAllConcerts(int page, int size)
        {
            IEnumerable<Concert> result;
            if (page > 0 && size > 0)
            {
                int offset = (page - 1) * size;
                result = _context.Concert.Skip(offset).Take(size);
            }
            else
                result = _context.Concert;
            return result;
        }

        public async Task<Concert> GetConcert(int id)
        {
            return await _context.Concert.FindAsync(id);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public void ChangeState(Concert concert, EntityState state)
        {
            _context.Entry(concert).State = state;
        }

        public EntityState AddConcert(Concert concert)
        {
            return _context.Concert.Add(concert).State;
        }

        public bool ConcertExists(int id)
        {
            return _context.Concert.Any(e => e.Id == id);
        }
    }
}
