import { Component, OnInit } from '@angular/core';
import { Message } from '../_models/message';
import { Pagination } from '../_models/pagination';
import { MessageService } from '../_services/message.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {

  messages: Message[];
  pagination: Pagination;
  container= "Inbox";
  pagenumber= 1;
  pagesize= 5;

  constructor(private messageService: MessageService) {

   }

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages(){
    this.messageService.getmessage(this.pagenumber,this.pagesize, this.container)
    .subscribe((res)=>{
      this.messages = res.result;
      this.pagination = res.pagination;
    })
  }

  pageChanged(event: any){
    if(this.pagenumber !== event.page){
      this.pagenumber = event.page;
      this.loadMessages();
    }
  }

}
