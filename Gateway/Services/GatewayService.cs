﻿using Gateway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Services
{
    public class GatewayService : IGatewayService
    {
        private readonly IConcertService concertService;
        private readonly IPerfomersService perfomersService;
        private readonly IVenuesService venuesService;

        public GatewayService(IConcertService _concertService, 
            IPerfomersService _perfomersService, IVenuesService _venuesService)
        {
            concertService = _concertService;
            perfomersService = _perfomersService;
            venuesService = _venuesService;
        }

        public async Task<List<Concert>> GetConcerts(int page, int size)
        {
            return await concertService.GetAll(page, size);
        }

        public async Task<Perfomer> GetPerfomerById(int id)
        {
            return await perfomersService.GetById(id);
        }

        public async Task<Venue> GetVenueById(int id)
        {
            return await venuesService.GetById(id);
        }

        public async Task<(bool, Concert)> PostConcert(Concert concert)
        {
            return await concertService.PostOne(concert);
        }

        public async Task<bool> PostSchedule(Schedule schedule)
        {
            return await venuesService.PostSchedule(schedule);
        }

        public async Task<bool> PutConcert(int id, Concert concert)
        {
            return await concertService.PutOne(id, concert);
        }

        public async Task<bool> PutSchedule(Schedule schedule)
        {
            return await venuesService.PutSchedule(schedule);
        }

    }
}