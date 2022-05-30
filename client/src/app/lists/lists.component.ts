import { Component, OnInit } from '@angular/core';
import { Member } from '../_models/member';
import { MembersService } from '../_services/members.service';
import { Pagination } from '../_models/pagination';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {

  members: Partial<Member[]>;
  predicate: string = "liked";
  pagenumber= 1;
  pagesize = 5;
  pagination: Pagination;

  constructor(private memberService: MembersService) { }

  ngOnInit(): void {
    this.loadLikes();
  }

  loadLikes(){
    this.memberService.getLikes(this.predicate,this.pagenumber, this.pagesize).subscribe(res=>{
      this.members = res.result;
      this.pagination = res.pagination;
    })
  }

  pageChanged(event: any){
    this.pagenumber = event.page;
    this.loadLikes();
  }

}
