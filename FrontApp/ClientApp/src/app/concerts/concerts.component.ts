import { Component, OnInit } from '@angular/core';
import { ConcertsService } from '../concerts.service';
import { Concert } from '../concert';
import { Perfomer } from '../perfomer';
import { Venue } from '../venue';
import { ConcertGet } from '../concertGet';
import { MessageService } from '../message.service';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ConcertsPage } from '../concertsPage';


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
  concertP: ConcertsPage;

  itemsPerPage: number;
  totalItems: any;
  page: any;
  previousPage: any;

  constructor(private dataService: ConcertsService,
    public messageService: MessageService) { }

  ngOnInit() {
       // загрузка данных при старте компонента
    this.dataService.getPerfomers()
      .subscribe((data: Perfomer[]) => this.perfomers = data);
    this.dataService.getVenues()
      .subscribe((data: Venue[]) => this.venues = data);
    this.page = 1;
    this.itemsPerPage = 10;
    this.totalItems = 10;
    this.loadPage(this.page);
  }

  loadPage(page: number) {
    this.changing = false;
    if (page !== this.previousPage) {
      this.previousPage = page;
      this.loadConcerts();
    }
  }

  

  // получаем данные через сервис
  loadConcerts() {
    this.dataService.getConcerts(this.page, this.itemsPerPage)
      .subscribe((data: ConcertsPage) => {
      this.concerts = data.concertsData,
        this.totalItems = data.totalCount,
        this.page = data.page,
        this.itemsPerPage = data.pageSize});
    
  }
  
  onSelect(selConcert: ConcertGet) {
    this.selectedConcert = selConcert;
   
  }
  onPageChanged(value: any) {
    this.selectedConcert = new ConcertGet(1, "", "", "", new Date());
    this.changing = value;
    this.loadConcerts();
  }

  
}
