import { Component, OnInit } from '@angular/core';
import {AccountService} from '../_services/account.service'
import { Router } from '@angular/router';


@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {};
  //intead of using this flag to chec if the user is logged in or not 
  //we can directly use the service subject which we are using to check
  //if the user is there are not in the local storage. that's why commenting
  //the related code.
  
  //isLoggedIn: boolean;

  //made this service public to use it in the template.
  constructor(public accountService: AccountService, private router: Router) { }

  ngOnInit(): void {
   // this.getCurrentUser();
  }

  OnSubmit(){
    this.accountService.login(this.model).subscribe(response=>{
    this.router.navigate(['/members']);
      // console.log(response);
      //this.isLoggedIn = true;
    },
    err=>{
      console.log(err);
    });
  }

  onLogout(){
    this.accountService.logout();
   // this.isLoggedIn = false;
   this.router.navigate(['/']);
  }

  //we are using the service subject in template so no need of this method anymore
  // getCurrentUser(){
  //   this.accountService.currentUser$.subscribe(user=>{
  //     this.isLoggedIn = !!user;
  //   },
  //   err=>{
  //     console.log(err);
  //   });
  // }

}
