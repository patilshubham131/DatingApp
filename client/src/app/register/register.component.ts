import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { FormGroup, FormControl, Validators, ValidatorFn, AbstractControl, FormBuilder } from '@angular/forms';

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
  maxdate: Date;

  registerForm: FormGroup;
  constructor(private accService: AccountService, private toastrService: ToastrService,
    private fb: FormBuilder) { }

  ngOnInit(): void {
    this.initializeForm();
    this.maxdate = new Date();
    this.maxdate.setFullYear(this.maxdate.getFullYear() -18);
  }

  initializeForm(){
    this.registerForm = this.fb.group({
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      gender: ['male'],
      password: ['',[Validators.required, Validators.minLength(4)]],
      confirmPassword: ['',[Validators.required, this.matchValue('password')]]
    })

    this.registerForm.controls.password.valueChanges.subscribe(()=>{
      this.registerForm.controls.confirmPassword.updateValueAndValidity();
    })
  }

  matchValue(matchTo: string): ValidatorFn{

    return (control: AbstractControl) =>{

      return control?.value === control?.parent?.controls[matchTo].value ? 
      null : {ismatching: true}
    }
  }

  cancel(){
    this.cancelRegister.emit(false);    
  }

  onSubmit(){
    //console.log(this.model);
    console.log(this.registerForm.value);
    // this.accService.register(this.model).subscribe(res=>{
    //   console.log(res);
    //   this.cancel();
    // },err=>{
    //   console.log(err)
    //   this.toastrService.error(err.error);
    // })
  }

}
