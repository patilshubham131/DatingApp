import { Component, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';
import { Observable } from 'rxjs';
import { Pagination } from 'src/app/_models/pagination';
import { UserParams } from 'src/app/_models/userParams';
import { AccountService } from 'src/app/_services/account.service';
import { User } from 'src/app/_models/User';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

   members: Member[];
   pagenumber = 1;
   pagesize = 5;
   pagination: Pagination;
  //members$: Observable<Member[]>;

  userparams: UserParams;
  user: User;


  constructor(private memberService: MembersService, private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user=>{
      this.user = user;
      this.userparams = new UserParams(user);
    })
   }

  ngOnInit(): void {
   // this.members$ = this.memberService.getMembers();
  this.loadMembers();
  }

  loadMembers(){
    console.log("current page :" + this.pagenumber);
    // this.memberService.getMembers(this.pagenumber, this.pagesize).subscribe(res=>{
    //   this.members = res.result;
    //   this.pagination = res.pagination;
    // })

    this.memberService.getMembers(this.userparams).subscribe(res=>{
      this.members = res.result;
      this.pagination = res.pagination;
    })
  }

  pageChanged(event: any){
    // console.log(event.page);
    this.userparams.pageNumber = event.page;
    this.loadMembers();
  }

}
