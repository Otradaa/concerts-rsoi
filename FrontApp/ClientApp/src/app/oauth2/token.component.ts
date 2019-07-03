import { Component, Inject, Injectable } from '@angular/core'; 

//import { User } from './shared/user'; 
import { UserTokens } from '../userstoken'; 
import { HttpClient, HttpHeaders } from '@angular/common/http'; 

import { Router, ActivatedRoute } from '@angular/router'; 
import { first } from 'rxjs/operators';


//import { Observable } from 'rxjs'; 
//import { retry, map, catchError } from 'rxjs/operators'; 
//import { _throw as throwError } from 'rxjs/observable/throw'; 

//import { FormBuilder, FormGroup, Validators } from '@angular/forms'; 

@Component({
  selector: 'app-oauth2',
  templateUrl: './token.component.html'
})

export class TokenComponent {
  //error: any; 


  //loginForm: FormGroup; 
  submitClick = false;
  submitted = false;
  returnUrl: string;
  error = '';
  // user: User; 
  usersToken: UserTokens;
  http: HttpClient;
  authCode: string;

  constructor(http: HttpClient, private route: ActivatedRoute,
    private router: Router) {

    // this.baseUrl = "https://localhost:44350/api/products"; 
    this.http = http;
    this.returnUrl = localStorage.getItem('returnUrl');

  }


  ngOnInit() {
    // this.user = new User(); 
    this.usersToken = new UserTokens();
    this.authCode = this.route.snapshot.queryParams['code'];

    let url = 'https://localhost:44366/api/auth/token'//?' + `code=${this.authCode}`;// + `&client_sercret=secret&client_id=app&redirect_uri=http://localhost:44323/` ; 

    const myHeaders = new HttpHeaders().set('Content-Type', 'application/json');

    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };

    /* let options;
     options["headers"] = new HttpHeaders()
       .append('Content-Type', 'application/json');*/
    var bodyv = `{
           "client_id" : ` + `"clientid"` + `,
           "appSecret" : ` + `"secret"` + `,
           "code" : "` + this.authCode + `"
           }`;

    this.http.post<UserTokens>(url, bodyv, httpOptions)
      .subscribe(
        result => {
          this.usersToken = result;
          localStorage.setItem('TokenInfo', JSON.stringify(result));
          this.router.navigate([this.returnUrl]);
        }, error => { console.error(error); });
    // this.getProducts(3, 0); 
  }



}
