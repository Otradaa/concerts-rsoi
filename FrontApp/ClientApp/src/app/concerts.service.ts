import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Concert } from './concert';
import { ConcertGet } from './concertGet';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { MessageService } from './message.service';
import { Perfomer } from './perfomer';
import { Venue } from './venue';

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

  constructor(private http: HttpClient, private messageService: MessageService) {
  }

  getConcerts() {
    return this.http.get(this.concertUrl).pipe(
      tap((concertAns: ConcertGet[]) => console.log(`got concerts`)),
      catchError(this.handleError<ConcertGet[]>('getConcerts'))
    );
  }

  getPerfomers() {
    return this.http.get(this.perfomerUrl).pipe(
      tap((concertAns: Perfomer[]) => console.log(`got perfomers`)),
      catchError(this.handleError<Perfomer[]>('getPerfomers'))
    );
  }

  getVenues() {
    return this.http.get(this.venueUrl).pipe(
      tap((concertAns: Venue[]) => console.log(`got venues`)),
      catchError(this.handleError<Venue[]>('getVenues'))
    );
  }

  createConcert(concert: Concert): Observable<Concert> {
    var bodyv = `{
           "perfomerId" : ` + concert.perfomerId + `,
           "venueId" : ` + concert.venueId + `,
           "date" : "` + concert.date + `"
           }`;
    return this.http.post<Concert>(this.concertUrl, bodyv, httpOptions).pipe(
      tap((concertAns: Concert) => console.log(`added concert w/ id=${concertAns.id}`)),
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
    this.messageService.add(`ConcertService: ${message}`);
  }
  updateConcert(concert: Concert) {

    return this.http.put(this.concertUrl + '/' + concert.id, concert).pipe(
      tap((concertAns: Concert) => console.log(`edited concert w/ id=${concertAns.id}`)),
      catchError(this.handleError<Concert>('updatedConcert'))
    );
  }
  /*deleteConcert(id: number) {
    return this.http.delete(this.url + '/' + id);
  }*/
}
