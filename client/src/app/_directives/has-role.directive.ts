import { Directive, ViewContainerRef, TemplateRef, OnInit, Input } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { User } from '../_models/User';
import { take } from 'rxjs/operators';

@Directive({
  selector: '[appHasRole]'
})
export class HasRoleDirective implements OnInit {

  @Input() appHasRole: string[];
  user : User;

  constructor(private viewcontainerref : ViewContainerRef, private templateref : TemplateRef<any>, 
    private accountService: AccountService) {
      this.accountService.currentUser$.pipe(take(1)).subscribe(user=>{
        this.user = user;
      })

     }
  ngOnInit(): void {
    //clear view if no roles
    if(!this.user?.roles || this.user == null){
      this.viewcontainerref.clear();
      return;
    }

    if(this.user?.roles.some(r => this.appHasRole.includes(r))){
      this.viewcontainerref.createEmbeddedView(this.templateref);
    }
    else{
      this.viewcontainerref.clear();
    }
  }

}
