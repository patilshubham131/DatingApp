import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {map} from 'rxjs/operators'
import { ReplaySubject } from 'rxjs';
import { User } from '../_models/User';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  baseUrl = environment.apiUrl;
  private currentUser = new ReplaySubject<User>(1);

  currentUser$ = this.currentUser.asObservable();

  constructor(private http: HttpClient) { }

  login(model: any)
  {
    return this.http.post(this.baseUrl + 'account/login', model)
    .pipe(
        map((response: User)=>
            {
              const user = response;
              if(user){
                this.setCurrentUser(user);
              }
            }
          )
        );
  }

  setCurrentUser(user: User){
    user.roles = [];
    const roles = this.getDecodedToken(user.token).role;

    Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUser.next(user);
  }

  logout(){
    localStorage.removeItem('user');
    this.currentUser.next(null);
  }


  register(model: any){
    return this.http.post(this.baseUrl+'account/register',model).pipe(
      map((user: User)=>{
        if(user){
          this.setCurrentUser(user);
        }
        return user;
      })
    );
  }

  getDecodedToken(token){
   return JSON.parse(atob(token.split('.')[1]));
  }

}
