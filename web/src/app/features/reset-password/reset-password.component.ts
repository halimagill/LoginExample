import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { passwordMatchValidator } from '../../core/validators/passwordMatch.validators';

@Component({
  selector: 'reset-password',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.less'
})
export class ResetPasswordComponent {
  resetPasswordForm: FormGroup;

  constructor(fb:FormBuilder) { 
    this.resetPasswordForm = fb.group({
      oldPassword: ['', ],
      password: ['', [Validators.required, Validators.minLength(8), Validators.pattern('(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{8,}')]],
      passwordConfirm: ['', [Validators.required, Validators.minLength(8), passwordMatchValidator]]
    });
  }

  get oldPassword() { 
    return this.resetPasswordForm.get('oldPassword');
  }

  get password() { 
    return this.resetPasswordForm.get('password');
  }

  get passwordConfirm() { 
    return this.resetPasswordForm.get('passwordConfirm');
  }

}
