import { Component, OnInit, Input } from '@angular/core';
import { ConcertsService } from '../concerts.service';


import { Concert } from '../concert';
import { Perfomer } from '../perfomer';
import { Venue } from '../venue';
import { ConcertGet } from '../concertGet';
import { ConcertFull } from '../concertFull';

@Component({
  selector: 'app-concert-detail',
  templateUrl: './concert-detail.component.html',
  styleUrls: ['./concert-detail.component.css']
})
export class ConcertDetailComponent implements OnInit {

  @Input() concertInput: ConcertGet = new ConcertGet(1,"","","",new Date());
  concert: Concert = new Concert(1);
  fvenues: Venue[];
  fperfomers: Perfomer[];
  submitted = false;

  onSubmit() {
    this.submitted = true;
    this.dataService.updateConcert(this.concert).subscribe((concertN: Concert) => this.concert = concertN);
  }

  constructor(private dataService: ConcertsService) {
    this.concert.id = this.concertInput.id;
    this.dataService.getPerfomers()
      .subscribe((data: Perfomer[]) => this.fperfomers = data);
    this.dataService.getVenues()
      .subscribe((data: Venue[]) => this.fvenues = data);
  }

  ngOnInit() {
    

    
  }

  
  

}
