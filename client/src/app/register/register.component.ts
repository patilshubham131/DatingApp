import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  //this input property was there jst to practive communication 
  //from parent component to child component
 // @Input() usersFromHomeComp : any;
  @Output() cancelRegister = new EventEmitter();

  model: any ={};
  constructor(private accService: AccountService) { }

  ngOnInit(): void {
  }

  cancel(){
    this.cancelRegister.emit(false);    
  }

  onSubmit(){
    //console.log(this.model);
    this.accService.register(this.model).subscribe(res=>{
      console.log(res);
      this.cancel();
    },err=>console.log(err))
  }

}
