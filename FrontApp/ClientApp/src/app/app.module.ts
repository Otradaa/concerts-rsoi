import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';



import { httpInterceptor } from './token.interceptor';
import { ErrorInterceptor } from './error.interceptor';
import { TokenComponent } from './oauth2/token.component'
import { AuthComponent } from './auth/auth.component';
import { TokenCheck } from './auth/token.check';



import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ConcertsComponent } from './concerts/concerts.component';
import { ConcertDetailComponent } from './concert-detail/concert-detail.component';
import { ConcertCreateComponent } from './concert-create/concert-create.component';
import { MessagesComponent } from './messages/messages.component';
import { Oauth2Component } from './oauth2/oauth2.component';

//import { AuthComponent } from './auth/auth.component';

@NgModule({
  declarations: [
    AppComponent,
    ConcertsComponent,
    ConcertDetailComponent,
    ConcertCreateComponent,
    MessagesComponent,
    AuthComponent,
    Oauth2Component,
    TokenComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    FormsModule,
    NgbModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', redirectTo: '/login', pathMatch: 'full' },
      { path: 'concerts', component: ConcertsComponent },
      { path: 'concerts/new', component: ConcertCreateComponent },
      { path: 'concerts/:id', component: ConcertDetailComponent },
      { path: 'oauth2/login', component: Oauth2Component },
      { path: 'return', component: TokenComponent },
      { path: 'login', component: AuthComponent }
    ])
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: httpInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    TokenCheck //, AutorizationService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
