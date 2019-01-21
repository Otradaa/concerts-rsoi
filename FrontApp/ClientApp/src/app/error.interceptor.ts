import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

//import { AutorizationService } from '../autorization/autorization.service';
import { Router } from '@angular/router';
import { UsersToken } from './userstoken';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(/*private authenticationService: AutorizationService,*/ private http: HttpClient, private router: Router) { }

  intercept(request: HttpRequest<any>, newRequest: HttpHandler): Observable<HttpEvent<any>> {

    let tokenInfo = JSON.parse(localStorage.getItem('TokenInfo'));

    request = request.clone({
      setHeaders: {
        // Authorization: `Bearer ${tokenInfo.accessToken}`,
        // 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8'
      }
    });
    return newRequest.handle(request).pipe(catchError(err => {
      if (err.status === 401 && tokenInfo != null) {
        localStorage.removeItem('TokenInfo');
        //     request.s < UsersToken > = localStorage.getItem('TokenInfo');
        let url = 'https://localhost:44366/api/auth/refreshtokens';///////////////////////////////////////////////////////////////

        this.http.post<UsersToken>(url, tokenInfo)
          .subscribe(result => {
            localStorage.setItem('TokenInfo', JSON.stringify(result));
          }, error => { console.error(error); });

        // return  return newRequest.handle(request);
        //if 401 response returned from api, logout from application & redirect to login page.
        // this.authenticationService.logout();
      }

      const error = err.statusText || err.error.message ;
      return Observable.throw(error);
    }));
  }
}
