import { Component,  } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { AuthenticationService } from '../../core/services/authentication.service';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';

@Component({ 
  selector: 'mainlogin',
  imports: [ReactiveFormsModule, CommonModule],
  providers: [AuthenticationService],
  templateUrl: './login.component.html',
  styleUrl: './login.component.less'
})
export class LoginComponent {
  loginForm: FormGroup;
  isInvalidLogin: boolean = false;
  private _isUserLoggedIn$ = new BehaviorSubject<boolean>(false); 
  public isLoggedIn$ = this._isUserLoggedIn$.asObservable();

  constructor(private fb: FormBuilder,
              private authService: AuthenticationService,
              private router: Router) {

                this.loginForm = this.fb.group({
                  email: new FormControl('', [Validators.required, Validators.email]),
                  password: new FormControl('', Validators.required),
                });
  }

  get email() {
    return this.loginForm.get('email');
  }

  get password() {
    return this.loginForm.get('password');
  }

  goToCreateLogin() {
    debugger;
    this.router.navigateByUrl('/create-login');
  }

  login() {
    debugger;
    const val = this.loginForm.value;
    console.log(this.loginForm.value);

    this.authService.login(val.email, val.password)
    .subscribe((response : any) => {
      debugger;
      console.log(response); 
            
      if (this.isUserLogin(response.data)) {
        // const loginInfo: LoginInfo = response.data as LoginInfo;
        // this.userInfo = loginInfo;
        this._isUserLoggedIn$.next(true);
        localStorage.setItem('token', response.data.token);                    

        this.router.navigateByUrl('/dashboard');
        console.log("User is logged in");     
        // Use the user object here
      } else {
        this.isInvalidLogin = true;
        // Handle the case where the response is not a valid User object
        throw new Error('Invalid login: ' + JSON.stringify(response));
      }
    });   
  }

  // Handle the response here
  isUserLogin(response: any): boolean {
    return response && typeof response === 'object' 
    && 'token' in response;
  };

  resetPassword() {
  debugger;
    this.router.navigateByUrl('/reset-password');
  }
}
