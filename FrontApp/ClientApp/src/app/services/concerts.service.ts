import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Concert } from '../concert';
import { ConcertGet } from '../concertGet';
import { Observable, of, throwError  } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { MessageService } from '../message.service';
import { Perfomer } from '../perfomer';
import { Venue } from '../venue';
import { ConcertsPage } from '../concertsPage';
//import { SecurityService } from './auth.service';

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
  error='';

  constructor(private http: HttpClient, private messageService: MessageService) {//,
   // private securityService: SecurityService) {
  }

  getConcerts(page: any, size: number) {
    return this.http.get(this.concertUrl + `?page=` + page +`&pageSize=`+size).pipe(
      tap((concertAns: ConcertsPage) => console.log(`got concerts`)),
      catchError(this.handleError)
    );
  }

  getPerfomers() {
    return this.http.get(this.perfomerUrl).pipe(
      tap((concertAns: Perfomer[]) => console.log(`got perfomers`)),
      catchError(this.handleError)
    );
  }

  getVenues() {
    return this.http.get(this.venueUrl).pipe(
      tap((concertAns: Venue[]) => console.log(`got venues`)),
      catchError(this.handleError)
    );
  }

  createConcert(concert: Concert): Observable<Concert> {
    let options = {};
  //  this.setHeaders(options);
    var bodyv = `{
           "perfomerId" : ` + concert.perfomerId + `,
           "venueId" : ` + concert.venueId + `,
           "date" : "` + concert.date + `"
           }`;
    return this.http.post<Concert>(this.concertUrl, bodyv, options).pipe(
      tap((concertAns: Concert) => console.log(`added concert w/ id=${concertAns.id}`)),
      catchError(this.handleError)
    );
  }

  handleError(error) {
    let errorMessage = '';
    if (error.error instanceof ErrorEvent) {
      // client-side
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // server-side 
      if (error.status === undefined)
        return;
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
      
    }
    window.alert(errorMessage);
    return throwError(errorMessage);
  }


  /*private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      // TODO: send the error to remote logging infrastructure
       // log to console instead

      this.error = error.message;

      // TODO: better job of transforming error for user consumption
      this.log(`${operation} failed: ${error.message}`);
      console.error(error);
      // Let the app keep running by returning an empty result.
      return throwError(error);
    };
  }*/

  private log(message: string) {
    this.messageService.add(`ConcertService: ${message}`);
  }

  updateConcert(concert: Concert) {
    let options = {};
    //this.setHeaders(options);
    return this.http.put(this.concertUrl + '/' + concert.id, concert, options).pipe(
      tap(result => console.log(`edited concert`)),
      catchError(this.handleError)
    );
  }

  /*private setHeaders(options: any) {
    if (this.securityService) {
      options["headers"] = new HttpHeaders()
        .append('Content-Type', 'application/json' )
        .append('authorization', 'Bearer ' + this.securityService.GetToken());
    }
  }*/
}

