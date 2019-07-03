import { Component, Inject, Injectable, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { User } from '../user';
import { UserTokens, RedUrl } from '../userstoken';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { throwError } from 'rxjs';
//import { Observable } from 'rxjs';


@Component({
  selector: 'app-oauth2',
  templateUrl: './oauth2.component.html',
  styleUrls: ['./oauth2.component.css']
})
export class Oauth2Component implements OnInit {


  //loginForm: FormGroup;
  submitClick = false;
  submitted = false;
  returnUrl: string;
  error = '';
  user: User;
  usersToken: UserTokens = new UserTokens('', '');
  http: HttpClient;
  loginForm: FormGroup;
  loading = false;
  client_id: string;
  redirect_uri: string;
  response_type: string;
  redirect = '';
  // on this from auth service with redirect uri and client_id as query params
  // click submit and  POST /authcode client_id red_uri resp_type and user in body

  constructor(
    http: HttpClient,
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router) {
    this.http = http;
  }

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      console.log(params);
      this.client_id = params['client_id'];
      this.redirect_uri = params['redirect_uri'];
      this.response_type = params['response_type'];
    });
    this.user = new User();
    //this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  get f() { return this.loginForm.controls; }


  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.loginForm.invalid) {
      return;
    }

    let url = 'https://localhost:44366/api/auth/authcode?client_id=' + this.client_id +
      "&redirect_uri=" + this.redirect_uri + "&response_type=" + this.response_type;
    this.loading = true;
    this.user.username = this.f.username.value;
    this.user.password = this.f.password.value;
    this.http.post <RedUrl>(url, this.user)
     // .pipe(first())
      .subscribe(
      result => {
        //вынуть из ссылки код и сохранить а потом запросить токен?
        //вынуть код
        console.log(result);
        this.redirect = result.red;

        //window.location.href = result.headers.get('location');
        },
      error => {
        //if (error.code == 302)
        console.log(error);
          //window.location.href = error;
        
        this.error = error;

          this.loading = false;
      });

    //get token
  }

}
