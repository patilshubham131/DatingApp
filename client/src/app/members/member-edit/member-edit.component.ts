import { Component, OnInit, ViewChild } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/User';
import { MembersService } from 'src/app/_services/members.service';
import { AccountService } from 'src/app/_services/account.service';
import { take } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {

  member: Member;
  user: User;

  @ViewChild('editForm') editForm : NgForm;

  constructor(private accountService: AccountService, private memberService: MembersService, 
    private toastrService: ToastrService) { 

    this.accountService.currentUser$.pipe(take(1)).subscribe(
      result => this.user = result
    );
  }

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember(){
    this.memberService.getMember(this.user.userName).subscribe(member=> this.member = member);
  }

  updateMember(){
    console.log(this.member); 
    this.toastrService.show("User Profile updated..");
    this.editForm.reset(this.member);
  }
}
