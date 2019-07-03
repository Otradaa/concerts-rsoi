import { Component, OnInit } from '@angular/core';
import { ConcertsService } from '../services/concerts.service';
import { MessageService } from '../message.service';
import { NgForm } from '@angular/forms';

import { Concert } from '../concert';
import { Perfomer } from '../perfomer';
import { Venue } from '../venue';
import { ConcertGet } from '../concertGet';


@Component({
  selector: 'app-concert-create',
  templateUrl: './concert-create.component.html',
  styleUrls: ['./concert-create.component.css']
})
export class ConcertCreateComponent implements OnInit {

  concert: Concert = new Concert(null);
  createdConcert: Concert;
  perName = '';
  venName = '';
  fvenues: Venue[];
  fperfomers: Perfomer[];
  submitted = false;
  error='';

  constructor(private dataService: ConcertsService,
    public messageService: MessageService) {
    this.dataService.getPerfomers()
      .subscribe((data: Perfomer[]) => this.fperfomers = data);
    this.dataService.getVenues()
      .subscribe((data: Venue[]) => this.fvenues = data);
  }

  ngOnInit() {
  }

  onSubmit(form: NgForm) {
    
    if (this.concert.id == null) {
      this.dataService.createConcert(this.concert)
        .subscribe((data: Concert) => {
          this.createdConcert = data;
          this.submitted = true;
          this.error = '';
    },
        error => {
          this.error = "Server: Date is invalid";
          //this.error = error;
        });
    }
    

  }
  getPerfomer(id:number):string {
    for (let p of this.fperfomers)
      if (p.id === id)
        return p.name;
  }

  getVenue(id: number): string {
    for (let p of this.fvenues)
      if (p.id === id)
        return p.name;
  }

}
