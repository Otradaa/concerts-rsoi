import { Component, OnInit } from '@angular/core';
import { Concert } from '../concert';
import { Perfomer } from '../perfomer';
import { Venue } from '../venue';
import { ConcertGet } from '../concertGet';

@Component({
  selector: 'app-concerts',
  templateUrl: './concerts.component.html',
  styleUrls: ['./concerts.component.css']
})
export class ConcertsComponent implements OnInit {

  concert: Concert = new Concert();   
  perfomer: Perfomer = new Perfomer();
  venue: Venue = new Venue();
  concerts: ConcertGet[];
  selectedConcert: ConcertGet;
  venues: Venue[];
  perfomers: Perfomer[];

  constructor() { }

  ngOnInit() {
  }

  onSelect(selConcert: ConcertGet): void {
    this.selectedConcert = selConcert;
  }

}
