import { Component, OnInit, ViewChild } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from '@kolkov/ngx-gallery';
import { TabsetComponent, TabDirective } from 'ngx-bootstrap/tabs';
import { MessageService } from 'src/app/_services/message.service';
import { Message } from 'src/app/_models/message';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {

  @ViewChild('memberTabs', {static: true}) membertabs : TabsetComponent;
  
  member: Member;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  activatedTab : TabDirective;
  messages: Message[] = [];

  constructor(private memberService: MembersService, private route: ActivatedRoute, private messageService: MessageService) { }

  ngOnInit(): void {
    // this.loadMember();

    this.route.data.subscribe(data=>{
      this.member = data.member;
    })
    this.galleryOptions = [{
      width: '500px',
      height: '500px',
      imagePercent: 100,
      thumbnailsColumns: 4,
      imageAnimation: NgxGalleryAnimation.Slide,
      preview: false
    }]

    this.galleryImages = this.getImages();

    this.route.queryParams.subscribe(params=>{
      params.tab ? this.selectTab(params.tab) : this.selectTab(0);
    })
  }

  getImages(): NgxGalleryImage[]{

    const imgUrls = [];

    for(const photo of this.member.photos){
      imgUrls.push({small:photo.url,
      medium: photo.url,
    big: photo.url})
    }

    return imgUrls;
  }
  loadMember(){
    this.memberService.getMember(this.route.snapshot.paramMap.get('username')).subscribe(member=> {
      this.member = member;
      
    })
  }

  onTabActivated(data: TabDirective){
    this.activatedTab = data;

    if(this.activatedTab.heading === 'Messages' && this.messages.length === 0){
      this.loadMessages();
    }
  }

  loadMessages(){
    if(this.messages.length === 0){
      this.messageService.getMessageThread(this.member.userName).subscribe(messages=>{
        this.messages = messages;
      })
    }
  }

  selectTab(tab){
    this.membertabs.tabs[tab].active = true;
  }

}
