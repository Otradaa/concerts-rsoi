export class UsersToken {
  constructor(
    public refreshToken?: string,
    public accessToken?: string,
    public username?: string,

  ) { }
}
