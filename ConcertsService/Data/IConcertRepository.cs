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
        ConcertsCount GetAllConcerts(int page, int size);
        Task<Concert> GetConcert(int id);
        Task<int> GetCount();
        Task SaveChanges();
        void ChangeState(Concert concert, EntityState state);
        Concert AddConcert(Concert concert);
        bool ConcertExists(int id);
        void RemoveConcert(Concert concert);

    }
}
