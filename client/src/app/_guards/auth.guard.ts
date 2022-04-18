import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AccountService } from '../_services/account.service';
import { map } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  
  constructor(private accService: AccountService, private toastrService: ToastrService){}
  //this one is default method implementation we get after creating the guard with 
  //CLI. we can modify it as per our requirement. 
  canActivate(): Observable<boolean> {

    return this.accService.currentUser$.pipe(
      map(res=>{
        if(res){
          return true;
        }
        this.toastrService.error("You are not logged in.. please log in and try again..!");
      })
    )
  }
  
}
