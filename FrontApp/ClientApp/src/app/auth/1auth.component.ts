/*import { Component, OnInit, OnChanges, Output, Input, EventEmitter } from '@angular/core';
import { Subscription } from 'rxjs';

import { SecurityService } from '../services/auth.service';

@Component({
  selector: 'app-auth', //esh-identity
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css']
})
export class AuthComponent implements OnInit {

  authenticated: boolean = false;
  private subscription: Subscription;
  private userName: string = '';

  constructor(private service: SecurityService) {

  }

  ngOnInit() {
    this.subscription = this.service.authenticationChallenge$.subscribe(res => {
      this.authenticated = res;
      this.userName = this.service.UserData.name;////////////////was email
    });

    if (window.location.hash) {
      this.service.AuthorizedCallback();
    }

    console.log('identity component, checking authorized' + this.service.IsAuthorized);
    this.authenticated = this.service.IsAuthorized;

    if (this.authenticated) {
      if (this.service.UserData)
        this.userName = this.service.UserData.name;
    }
  }

  //logoutClicked(event: any) {
  //  event.preventDefault();
  //  console.log('Logout clicked');////////////////////////////////////////
  //  this.logout();
  //}

  login() {
    this.service.Authorize();
  }

  //logout() {
  //  this.signalrService.stop();///////////////////////////////////////////////////////////////////
  //  this.service.Logoff();
  //}

}
*/
