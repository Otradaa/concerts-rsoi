import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConcertCreateComponent } from './concert-create.component';

describe('ConcertCreateComponent', () => {
  let component: ConcertCreateComponent;
  let fixture: ComponentFixture<ConcertCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConcertCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConcertCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
