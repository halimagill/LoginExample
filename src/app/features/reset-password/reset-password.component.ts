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
      newPassword: ['', [Validators.required, Validators.minLength(8), Validators.pattern('(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{8,}')]],
      confirmPassword: ['', [Validators.required, Validators.minLength(8), passwordMatchValidator]]
    });
  }

  get oldPassword() { 
    return this.resetPasswordForm.get('oldPassword');
  }

  get newPassword() { 
    return this.resetPasswordForm.get('newPassword');
  }

  get confirmPassword() { 
    return this.resetPasswordForm.get('confirmPassword');
  }

}
