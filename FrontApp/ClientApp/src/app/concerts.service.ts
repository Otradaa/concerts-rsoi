import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Concert } from './concert';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})

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

  createConcert(concert: Concert): Observable<Concert> {
    var bodyv = `{
           "perfomerId" : ` + concert.perfomerId + `,
           "venueId" : ` + concert.venueId + `,
           "date" : "` + concert.date + `"
           }`;
    return this.http.post<Concert>(this.concertUrl, bodyv, httpOptions).pipe(
      tap((hero: Concert) => this.log(`added hero w/ id=${hero.id}`)),
      catchError(this.handleError<Concert>('createConcert'))
    );
  }
  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead

      // TODO: better job of transforming error for user consumption
      this.log(`${operation} failed: ${error.message}`);

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }

  /** Log a HeroService message with the MessageService */
  private log(message: string) {
    console.log(`ConcertService: ${message}`);
  }
  updateConcert(concert: Concert) {

    return this.http.put(this.concertUrl + '/' + concert.id, concert);
  }
  /*deleteConcert(id: number) {
    return this.http.delete(this.url + '/' + id);
  }*/
}
