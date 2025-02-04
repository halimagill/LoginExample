import { AbstractControl, ValidationErrors } from "@angular/forms";

export function passwordMatchValidator(AC: AbstractControl) : ValidationErrors | null {
    let password = AC.get('password'); // to get value in input tag
    let confirmPassword = AC.get('passwordConfirm'); // to get value in input tag

      if(!password || !confirmPassword) {
        return null;
      }

      if (password?.value !== confirmPassword?.value) {
        return { 'passwordMismatch': true };
      }

      return null; 
}

 

