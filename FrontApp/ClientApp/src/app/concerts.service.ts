import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Concert } from './concert';

@Injectable()
export class ConcertsService {

  private concertUrl = "https://localhost:44366/api/concerts";
  private perfomerUrl = "https://localhost:44366/api/perfomers";
  private venueUrl = "https://localhost:44366/api/venues";

  constructor(private http: HttpClient) {
  }

  getConcerts() {
    return this.http.get(this.concertUrl);
  }

  getPerfomers() {
    return this.http.get(this.perfomerUrl);
  }

  getVenues() {
    return this.http.get(this.venueUrl);
  }

  createConcert(concert: Concert) {
    return this.http.post(this.concertUrl, concert);
  }
  updateConcert(concert: Concert) {

    return this.http.put(this.concertUrl + '/' + concert.id, concert);
  }
  /*deleteConcert(id: number) {
    return this.http.delete(this.url + '/' + id);
  }*/
}
