import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DataService } from './data.services';
import { CreateLogin } from '../models/create-login.model';

@Injectable()
export class AuthenticationService extends DataService {
  private baseUrl: string = 'https://freeapi.miniprojectideas.com/api/User';
  
  constructor(http: HttpClient) {     
    super('https://freeapi.miniprojectideas.com/api/User', http);
    //TODO Move this to a storage service
   // const token = localStorage?.getItem('token');
    //TODO: Check if the token is valid and not expired    
   // this._isUserLoggedIn$.next(!!token);
  }
  
  createUser(user: CreateLogin) {
    return this.urlCreate('CreateNewUser', user);
  }

  // getUserByEmail(email: string) {

  // }

  login(email: string, password: string) {    
    return this.urlCreate('login', { "EmailId" : email, "Password": password })
  }
  
  // logout() {
  //   this._isUserLoggedIn$.next(false);
  //   localStorage.removeItem('token');
  // }
   
}
