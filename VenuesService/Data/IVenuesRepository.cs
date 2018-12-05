using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VenuesService.Models;

namespace VenuesService.Data
{
    public interface IVenuesRepository
    {
        Task<Venue> GetVenue(int id);
        Task<Schedule> GetSchedule(int id);
        Task SaveChanges();
        void ChangeState(Schedule schedule, Schedule newsch, EntityState state);
        void AddSchedule(Schedule schedule);
        bool ScheduleExists(int id);
        Schedule FirstSchedule(int? concertId);
    }
}
