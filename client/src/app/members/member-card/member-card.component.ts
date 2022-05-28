import { Component, OnInit, Input } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {

  @Input() member: Member;

  constructor(private memberservice: MembersService, private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  addLike(member: Member){
    this.memberservice.addLike(member.userName).subscribe(()=>{
      this.toastr.success("You have like " + member.knownAs);
    })
  }

  

}
