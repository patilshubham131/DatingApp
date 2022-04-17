import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {map} from 'rxjs/operators'
import { ReplaySubject } from 'rxjs';
import { User } from '../_models/User';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private currentUser = new ReplaySubject<User>(1);

  currentUser$ = this.currentUser.asObservable();

  constructor(private http: HttpClient) { }

  login(model: any)
  {
    return this.http.post('https://localhost:5001/api/account/login', model)
    .pipe(
        map((response: User)=>
            {
              const user = response;
              if(user){
                localStorage.setItem('user', JSON.stringify(user));
                this.currentUser.next(user);
              }
            }
          )
        );
  }

  setCurrentUser(user: User){
    this.currentUser.next(user);
  }

  logout(){
    localStorage.removeItem('user');
    this.currentUser.next(null);
  }


  register(model: any){
    return this.http.post('https://localhost:5001/api/account/register',model).pipe(
      map((user: User)=>{
        if(user){
          localStorage.setItem('user',JSON.stringify(user));
          this.currentUser.next(user);
        }
        return user;
      })
    );
  }

}
