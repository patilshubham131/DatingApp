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
  container= "Unread";
  pagenumber= 1;
  pagesize= 5;

  loading = false;

  constructor(private messageService: MessageService) {

   }

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages(){
    this.loading = true;
    this.messageService.getmessage(this.pagenumber,this.pagesize, this.container)
    .subscribe((res)=>{
      this.messages = res.result;
      this.pagination = res.pagination;
      this.loading = false;
    })
  }

  deleteMessage(id: number){
    this.messageService.deleteMessage(id).subscribe(()=>{
      this.messages.splice(this.messages.findIndex(m=> m.id == id), 1);
      
    })
  }

  pageChanged(event: any){
    if(this.pagenumber !== event.page){
      this.pagenumber = event.page;
      this.loadMessages();
    }
  }

}
