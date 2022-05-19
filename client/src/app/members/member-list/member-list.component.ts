import { Component, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';
import { Observable } from 'rxjs';
import { Pagination } from 'src/app/_models/pagination';

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


  constructor(private memberService: MembersService) { }

  ngOnInit(): void {
   // this.members$ = this.memberService.getMembers();
  this.loadMembers();
  }

  loadMembers(){
    this.memberService.getMembers(this.pagenumber, this.pagesize).subscribe(res=>{
      this.members = res.result;
      this.pagination = res.pagination;
    })
  }

}
