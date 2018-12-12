import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Concert } from './concert';

@Injectable()
export class ConcertsService {

  private url = "https://localhost:44366/api/concerts";

  constructor(private http: HttpClient) {
  }

  getConcerts() {
    return this.http.get(this.url);
  }

  createConcert(concert: Concert) {
    return this.http.post(this.url, concert);
  }
  updateConcert(concert: Concert) {

    return this.http.put(this.url + '/' + concert.id, concert);
  }
  /*deleteConcert(id: number) {
    return this.http.delete(this.url + '/' + id);
  }*/
}
