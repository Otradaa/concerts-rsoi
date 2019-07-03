import { ConcertGet } from './concertGet';

export class ConcertsPage {
  constructor(
    public totalCount?: number,
    public page?: number,
    public pageSize?: number,
    public concertsData?: ConcertGet[]) { }
}
