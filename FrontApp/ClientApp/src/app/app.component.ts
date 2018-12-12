import { Component, OnInit } from '@angular/core';
import { ConcertsService } from './concerts.service';
import { Concert } from './concert';
import { Perfomer } from './perfomer';
import { Venue } from './venue';
import { ConcertGet } from './concertGet';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [ConcertsService]
})
export class AppComponent implements OnInit {

  concert: Concert = new Concert();   // изменяемый товар
  concerts: ConcertGet[];                // массив товаров
  venues: Venue[];
  perfomers: Perfomer[];
  tableMode: boolean = true;          // табличный режим

  constructor(private dataService: ConcertsService) { }

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
  // сохранение данных
  save() {
    if (this.concert.id == null) {
      this.dataService.createConcert(this.concert)
        .subscribe((data: Concert) => this.concerts.push(data));
    } else {
      this.dataService.updateConcert(this.concert)
        .subscribe(data => this.loadConcerts());
    }
    this.cancel();
  }
  editConcert(p: Concert) {
    this.concert = p;
  }

  changeVenue(venId: number) {
    this.concert.venueId = venId;
  }

  changePerfomer(perId: number) {
    this.concert.perfomerId = perId;
  }

  cancel() {
    this.concert = new Concert();
    this.tableMode = true;
  }
 /* delete(p: Concert) {
    this.dataService.deleteConcert(p.id)
      .subscribe(data => this.loadConcerts());
  }*/
  add() {
    this.cancel();
    this.tableMode = false;
  }
}
