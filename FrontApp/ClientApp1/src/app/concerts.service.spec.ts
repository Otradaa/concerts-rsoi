import { TestBed, inject } from '@angular/core/testing';

import { ConcertsService } from './concerts.service';

describe('ConcertsService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ConcertsService]
    });
  });

  it('should be created', inject([ConcertsService], (service: ConcertsService) => {
    expect(service).toBeTruthy();
  }));
});
