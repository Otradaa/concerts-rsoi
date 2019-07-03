import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import { ConcertsService } from '../services/concerts.service';

import { NgForm } from '@angular/forms';

import { Concert } from '../concert';
import { Perfomer } from '../perfomer';
import { Venue } from '../venue';
import { ConcertGet } from '../concertGet';

@Component({
  selector: 'app-concert-detail',
  templateUrl: './concert-detail.component.html',
  styleUrls: ['./concert-detail.component.css']
})
export class ConcertDetailComponent implements OnInit {

  @Output()
  changed: EventEmitter<boolean> = new EventEmitter<boolean>();

  @Input() concertInput: ConcertGet;
  concert: Concert = new Concert(1);
  fvenues: Venue[];
  fperfomers: Perfomer[];
  submitted = false;
  intvenue: number;
  error = '';
  createdConcert: Concert;
  perName = '';
  venName = '';

  onSubmit(form: NgForm) {
    this.concert.id = this.concertInput.id;

    if (this.concert.perfomerId === undefined ||
      this.concert.perfomerId === null)
      this.getPerfomer();
    if (this.concert.venueId === undefined ||
      this.concert.venueId === null)
      this.getVenue();
    if (this.concert.date === undefined ||
      this.concert.date === null)
      this.concert.date = this.concertInput.date;

    this.dataService.updateConcert(this.concert).subscribe(result => {
      form.reset();
      this.submitted = true;
      this.error = '';
    },
      error => {
        this.error = "Server: Date is invalid";
        //this.error = error;
      });
  }

  constructor(private dataService: ConcertsService) {

    this.dataService.getPerfomers()
      .subscribe((data: Perfomer[]) => this.fperfomers = data);
    this.dataService.getVenues()
      .subscribe((data: Venue[]) => this.fvenues = data);
  }

  getPerfomer() {
    for (let p of this.fperfomers)
      if (p.name === this.concertInput.perfomerName)
        this.concert.perfomerId = p.id;
  }

  getVenue() {
    for (let p of this.fvenues)
      if (p.name === this.concertInput.venueName)
        this.concert.venueId = p.id;
  }
  
  ngOnInit() {
  }
}
