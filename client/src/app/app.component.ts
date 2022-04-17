import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from './_models/User';
import {AccountService} from './_services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'client';
  //users: any;

  constructor(private http: HttpClient, private accService: AccountService){}

  ngOnInit(): void {
    //this.getUsers();
    this.serCurrentUser();
  }

  //this method was just to test our users api.

  // getUsers(){
  //   this.http.get("https://localhost:5001/api/users").subscribe(
  //     response=>{
  //       this.users = response;
  //     },
  //     err=>{
  //       console.log(err);
  //     }
  //   )
  // }

  serCurrentUser(){
    const user: User = JSON.parse(localStorage.getItem('user')) ;
    this.accService.setCurrentUser(user);
  }

}
