
namespace FrontApp.ClientApp.src.app
{
  export class Concert {
    constructor(
      public id?: number,
      public perfomerName?: string,
      public venueName?: string,
      public venueAdress?: string,
      public date?: Date) { }
  }
}
