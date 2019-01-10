import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ConcertsComponent } from './concerts/concerts.component';
import { ConcertDetailComponent } from './concert-detail/concert-detail.component';
import { ConcertCreateComponent } from './concert-create/concert-create.component';

@NgModule({
  declarations: [
    AppComponent,
    ConcertsComponent,
    ConcertDetailComponent,
    ConcertCreateComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', redirectTo: '/concerts', pathMatch: 'full' },
      { path: 'concerts', component: ConcertsComponent },
      { path: 'concerts/new', component: ConcertCreateComponent },
      { path: 'concerts/:id', component: ConcertDetailComponent }
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
