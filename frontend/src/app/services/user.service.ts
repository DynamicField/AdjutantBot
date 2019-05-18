import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';

export interface CurrentUser {
  avatarHash: string;
  discordUsername: string;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) {}

  public getAuthenticatedUser(): Observable<CurrentUser> {
    return this.http.get<CurrentUser>('http://localhost:8080/user/me');
  }
}
