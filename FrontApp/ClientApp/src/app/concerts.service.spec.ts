import { TestBed } from '@angular/core/testing';

import { ConcertsService } from './services/concerts.service';

describe('ConcertsService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ConcertsService = TestBed.get(ConcertsService);
    expect(service).toBeTruthy();
  });
});
