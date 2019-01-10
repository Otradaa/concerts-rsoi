import { Component, OnInit } from '@angular/core';
import { ConcertsService } from '../concerts.service';
import { Concert } from '../concert';
import { Perfomer } from '../perfomer';
import { Venue } from '../venue';
import { ConcertGet } from '../concertGet';
import { MessageService } from '../message.service';

@Component({
  selector: 'app-concerts',
  templateUrl: './concerts.component.html',
  styleUrls: ['./concerts.component.css']
})
export class ConcertsComponent implements OnInit {

  concert: Concert = new Concert(1);   
  perfomer: Perfomer = new Perfomer();
  venue: Venue = new Venue();
  concerts: ConcertGet[];
  selectedConcert: ConcertGet = new ConcertGet(1,"","","",new Date());
  venues: Venue[];
  perfomers: Perfomer[];
  changing = false;

  constructor(private dataService: ConcertsService,
    public messageService: MessageService) { }

  ngOnInit() {
    this.loadConcerts();    // загрузка данных при старте компонента

  }
  // получаем данные через сервис
  loadConcerts() {
    this.dataService.getConcerts()
      .subscribe((data: ConcertGet[]) => this.concerts = data);
    this.dataService.getPerfomers()
      .subscribe((data: Perfomer[]) => this.perfomers = data);
    this.dataService.getVenues()
      .subscribe((data: Venue[]) => this.venues = data);
  }

  onSelect(selConcert: ConcertGet): void {
    this.selectedConcert = selConcert;
    
  }

}
