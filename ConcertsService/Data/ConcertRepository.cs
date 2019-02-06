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

        public Task<int> GetCount()
        {
            return _context.Concert.CountAsync();
        }

        public ConcertsCount GetAllConcerts(int page, int size)
        {
            ConcertsCount result = new ConcertsCount();
            if (page > 0 && size > 0)
            {
                int offset = (page - 1) * size;
                result.concerts = new List<Concert>(_context.Concert.Skip(offset).Take(size));
            }
            else
                result.concerts = new List<Concert>(_context.Concert);
            result.count = _context.Concert.Count();
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

        public Concert AddConcert(Concert concert)
        {
            return _context.Concert.Add(concert).Entity;
        }

        public bool ConcertExists(int id)
        {
            return _context.Concert.Any(e => e.Id == id);
        }

        public void RemoveConcert(Concert concert)
        {
            _context.Concert.Remove(concert);
        }
    }
}
