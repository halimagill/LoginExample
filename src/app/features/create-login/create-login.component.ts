import { Component } from '@angular/core';
import { AuthenticationService } from '../../core/services/authentication.service';
import { FormGroup, FormBuilder, FormControl,Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CreateLogin } from '../../core/models/create-login.model';
import { CommonModule } from '@angular/common';
import { passwordMatchValidator } from '../../core/validators/passwordMatch.validators';

@Component({
  selector: 'create-login',
  imports: [ReactiveFormsModule, CommonModule],
  providers: [AuthenticationService],
  templateUrl: './create-login.component.html',
  styleUrl: './create-login.component.less'
})
export class CreateLoginComponent {
 registerForm : FormGroup;
 errorMessage:string = '';
 hasError: boolean = false;
  

  constructor(private fb: FormBuilder
              , private authService: AuthenticationService
              , private router: Router) {

    this.registerForm = this.fb.group({
      firstName: new FormControl('', [Validators.required, Validators.minLength(3)]),
      lastName: new FormControl('', [Validators.required, Validators.minLength(3)]),
      emailId: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required, Validators.minLength(8), Validators.pattern('(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{8,}')]),
      passwordConfirm: new FormControl('', [Validators.required, passwordMatchValidator ])
    });    
  }

  get firstName() { 
    return this.registerForm.get('firstName');
  }
  get lastName() { 
    return this.registerForm.get('lastName');
  }
  get emailId() { 
    return this.registerForm.get('emailId');
  }
  get password() { 
    return this.registerForm.get('password');
  }
  get passwordConfirm() { 
    return this.registerForm.get('passwordConfirm');
  }
  
//HAVE TO MOVE SUBSCRIBE TO COMPONENT
  createLogin() {
    debugger;
    console.log(this.registerForm.value);
    const val = this.registerForm.value;

    if(this.registerForm.valid) {
          const newLogin: CreateLogin = {
            userId : 0,
            firstName : val.firstName,
            middleName : "N/A",
            lastName : val.lastName,
            emailId : val.emailId,
            password : val.password,
            mobileNo : "N/A",
            altMobileNo : "N/A"
        };

        this.authService.createUser(newLogin)
        .subscribe((response:any) => {
          debugger;
          console.log(response);

          if(response.success) {
            console.log("User is created");
            this.router.navigateByUrl('/login');
          }
          else
          {
            this.hasError = true;
            this.errorMessage = response.message;
            console.log("User is not created");
          }
        });        
    }
  }
}
