import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-test-errors',
  templateUrl: './test-errors.component.html',
  styleUrls: ['./test-errors.component.css']
})
export class TestErrorsComponent implements OnInit {

  //this is test component which is use to test the exception middleware and 
  //buggy controler at server side. 

  baseUrl = "https://localhost:5001/api/";
  constructor(private http: HttpClient) { }

  ngOnInit(): void {
  }

  get404Error(){
    this.http.get(this.baseUrl + "buggy/not-found").subscribe(res=>{
     console.log(res); 
    },err=>{
      console.log(err);
    })
  }

  get400Error(){
    this.http.get(this.baseUrl + "buggy/bad-request").subscribe(res=>{
     console.log(res); 
    },err=>{
      console.log(err);
    })
  }

  get500Error(){
    this.http.get(this.baseUrl + "buggy/server-error").subscribe(res=>{
     console.log(res); 
    },err=>{
      console.log(err);
    })
  }

  get401Error(){
    this.http.get(this.baseUrl + "buggy/auth").subscribe(res=>{
     console.log(res); 
    },err=>{
      console.log(err);
    })
  }

  get400Validation(){
    this.http.post(this.baseUrl + "account/register",{}).subscribe(res=>{
     console.log(res); 
    },err=>{
      console.log(err);
    })
  }

}
