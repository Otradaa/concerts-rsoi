import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { catchError, finalize, switchMap, filter, take } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { Router } from '@angular/router';
import { UserTokens } from './userstoken';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  isRefreshingToken: boolean = false;
  tokenSubject: BehaviorSubject<string> = new BehaviorSubject<string>(null);

  constructor(private http: HttpClient, private router: Router) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    let tokenInfo = JSON.parse(localStorage.getItem('TokenInfo'));
    if (tokenInfo)
      return next.handle(this.addTokenToRequest(request, tokenInfo.token))
        .pipe(
          catchError(err => {
            if (err instanceof HttpErrorResponse) {
              switch ((<HttpErrorResponse>err).status) {
                case 401:
                  return <Observable<HttpEvent<any>>>this.handle401Error(request, next);

              }
            } else {
              return throwError(err);
            }
          }));
    return next.handle(request);
  }

  private addTokenToRequest(request: HttpRequest<any>, token: string): HttpRequest<any> {
    return request.clone({ setHeaders: { Authorization: `Bearer ${token}` } });
  }

  private handle401Error(request: HttpRequest<any>, next: HttpHandler) {

    if (!this.isRefreshingToken) {
      this.isRefreshingToken = true;

      this.tokenSubject.next(null);

      let url = 'https://localhost:44366/api/auth/refresh';
      let tokenInfo = JSON.parse(localStorage.getItem('TokenInfo'));
      return this.http.post(url, tokenInfo)
        .pipe(
          switchMap((user: UserTokens) => {
            if (user) {
              this.tokenSubject.next(user.token);;
              localStorage.setItem('TokenInfo', JSON.stringify(user));
              return next.handle(this.addTokenToRequest(request, user.token));
            }

            return;
          }),
          catchError(err => {
            return err;
          }),
          finalize(() => {
            this.isRefreshingToken = false;
          })
        );
    } else {
      this.isRefreshingToken = false;

      return this.tokenSubject
        .pipe(filter(token => token != null),
          take(1),
          switchMap(token => {
            return next.handle(this.addTokenToRequest(request, token));
          }));
    }
  }
}
