export class UserTokensс {
  constructor(
    public refreshToken?: string,
    public accessToken?: string,
    public username?: string,

  ) { }
}

export class UserTokens {
  constructor(
    public refreshToken?: string,
    public token?: string,

  ) { }
}

export class RedUrl {
  constructor(
    public red?: string

  ) { }
}
