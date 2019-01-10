import { Component, OnInit } from '@angular/core';
import { ConcertsService } from '../concerts.service';


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
  fvenues: Venue[];
  fperfomers: Perfomer[];
  submitted = false;
  result;

  constructor(private dataService: ConcertsService) {
    this.dataService.getPerfomers()
      .subscribe((data: Perfomer[]) => this.fperfomers = data);
    this.dataService.getVenues()
      .subscribe((data: Venue[]) => this.fvenues = data);
  }

  ngOnInit() {
  }

  onSubmit() {
    this.submitted = true;

    if (this.concert.id == null) {
      this.dataService.createConcert(this.concert)
        .subscribe((data: Concert) => this.concert = data);
    }
  }

}
