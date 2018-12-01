using ConcertsService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConcertsService.Data
{
    public interface IConcertRepository
    {
        Task<Concert> GetConcert(int id);
        Task SaveChanges();
        void ChangeState(Concert concert, EntityState state);
        EntityState AddConcert(Concert concert);
        bool ConcertExists(int id);

    }
}
