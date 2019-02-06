﻿using Gateway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gateway.Services
{
    public class GatewayService : IGatewayService
    {
        private readonly IConcertService concertService;
        private readonly IPerfomersService perfomersService;
        private readonly IVenuesService venuesService;
        private readonly IAuthService authService;

        public GatewayService(IConcertService _concertService, IAuthService _authService,
            IPerfomersService _perfomersService, IVenuesService _venuesService)
        {
            concertService = _concertService;
            perfomersService = _perfomersService;
            venuesService = _venuesService;
            authService = _authService;
        }

        public async Task<int> GetConcertsCount()
        {
            return await concertService.GetCount();
        }
        public async Task<ConcertsCount> GetConcerts(int page, int size)
        {
            return await concertService.GetAll(page, size);
        }

        public async Task<List<Perfomer>> GetPerfomers()
        {
            return await perfomersService.GetAll();
        }
        public async Task<List<Venue>> GetVenues()
        {
            return await venuesService.GetAll();
        }

        public async Task<Perfomer> GetPerfomerById(int id)
        {
            var perfomer = await perfomersService.GetById(id);
            if (perfomer == null)
                perfomer = new Perfomer { };
            return perfomer;
        }

        public async Task<Venue> GetVenueById(int id)
        {
            var venue = await venuesService.GetById(id);
            if (venue == null)
                venue = new Venue { };
            return venue;
        }

        public async Task<ClientToken> GetToken()
        {
            return await concertService.GetToken();
        }
        public async Task<HttpResponseMessage> PostConcert(Concert concert, ClientToken token)
        {
            return await concertService.PostOne(concert, token);
        }

        public async Task<HttpResponseMessage> PostSchedule(Schedule schedule)
        {
            return await venuesService.PostSchedule(schedule);
        }

        public async Task<HttpResponseMessage> PutConcert(int id, Concert concert)
        {
            return await concertService.PutOne(id, concert);
        }

        public async Task<bool> PutSchedule(Schedule schedule)
        {
            return await venuesService.PutSchedule(schedule);
        }

        public async Task<bool> ValidateToken(string token)
        {
            return await authService.ValidateToken(token);
        }

        public async Task DeleteConcert(int id)
        {
            await concertService.DeleteConcert(id);
        }

    }
}
