import { Component, Inject, Injectable, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { User } from '../user';
import { UserTokens } from '../userstoken';
import { HttpClient } from '@angular/common/http';

//import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'login-data',
  templateUrl: './auth.component.html'
})

export class AuthComponent {
  //error: any;


  //loginForm: FormGroup;
  submitClick = false;
  submitted = false;
  returnUrl: string;
  error = '';
  user: User;
  usersToken: UserTokens = new UserTokens('','');
  http: HttpClient;
  loginForm: FormGroup;
  loading = false;
  url = 'https://localhost:44366/api/auth/authorize';//auth service

  constructor(
    http: HttpClient, 
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router) {
    this.http = http;
  }

  ngOnInit() {
    this.user = new User();
    this.usersToken = new UserTokens();
    this.returnUrl = '/concerts';
    localStorage.setItem('returnUrl', this.returnUrl);
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  get f() { return this.loginForm.controls; }

  authAPI() {
    //redirest to /oauth2
    //usual form there
    
    this.http
      .get<any>(this.url, { observe: 'response' })
      .subscribe(resp => {
        console.log(resp.headers.get('location'));
      });
    //window.location.href =;
  }



  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.loginForm.invalid) {
      return;
    }
    
    let url = 'https://localhost:44366/api/auth/login';
    this.loading = true;
    this.user.username = this.f.username.value;
    this.user.password = this.f.password.value;
    this.http.post<UserTokens>(url, this.user)
      .pipe(first())
      .subscribe(
      result => {
        this.usersToken = result;
        localStorage.setItem('TokenInfo', JSON.stringify(result));
          this.router.navigate([this.returnUrl]);
        },
        error => {
          this.error = error;
          this.loading = false;
        });
  }


  /*add() {


    // this.submitted = true;

    //this.submitClick = true;


    let url = 'https://localhost:44366/api/auth/login';//'https://localhost:44350/api/autorization';////////////////////////////////////////

    this.http.post<User>(url, this.user)
      .subscribe(result => {
        
        this.router.navigate([this.returnUrl]);
      }, error => { console.error(error); });



  }*/




}




