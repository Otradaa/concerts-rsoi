using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VenuesService.Models;

namespace VenuesService.Data
{
    public class VenuesRepository : IVenuesRepository
    {
        private readonly VenuesContext _context;

        public VenuesRepository(VenuesContext context)
        {
            _context = context;
        }

        public async Task<Venue> GetVenue(int id)
        {
            return await _context.Venue.FindAsync(id);
        }

        public async Task<Schedule> GetSchedule(int id)
        {
            return await _context.Schedule.FindAsync(id);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public void ChangeState(Schedule _schedule, Schedule schedule, EntityState state)
        {
            _schedule.Date = schedule.Date;
            _schedule.VenueId = schedule.VenueId;
            _context.Entry(_schedule).State = state;
        }

        public void AddSchedule(Schedule schedule)
        {
            _context.Schedule.Add(schedule);
        }

        public bool ScheduleExists(int id)
        {
            return _context.Schedule.Any(e => e.Id == id);
        }

        public Schedule FirstSchedule(int? concertId)
        {
            return _context.Schedule.First(s => s.ConcertId == concertId);
        }
    }
}
