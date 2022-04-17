import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  registrationMode: boolean;
  // users: any;

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    // this.getUsers();
  }

  toggleRegistrationMode(){
    this.registrationMode = !this.registrationMode;
  }

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
  cancelRgister(event: boolean){
    this.registrationMode = event;
  }

}
