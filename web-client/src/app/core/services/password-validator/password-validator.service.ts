import { Injectable } from '@angular/core';
import { AbstractControl, FormGroup, ValidatorFn } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class PasswordValidatorService {

  constructor() { }
  ConfirmedPasswordValidator(controlName: string, matchingControlName: string) {
    return (formGroup: FormGroup) => {
      const control = formGroup.controls[controlName];
      const matchingControl = formGroup.controls[matchingControlName];
      if (matchingControl.errors && !matchingControl.errors['confirmedValidator']) {
        return;
      }
      if (control.value !== matchingControl.value) {

        // this.form.controls['confirm_password'].setErrors({ confirmedValidator: true });
        matchingControl.setErrors({ confirmedValidator: true });
      } else {

        matchingControl.setErrors(null);
      }
    }
  }
  public validateConfirmPassword = (passwordControl: AbstractControl): ValidatorFn | null => {
    return (confirmationControl: AbstractControl): { [key: string]: boolean } | null => {
      const confirmValue = confirmationControl.value;
      const passwordValue = passwordControl.value;
      if (confirmValue === '') {
        return null;
      }
      if (confirmValue !== passwordValue) {
        return { mustMatch: true }
      }
      return null;
    };
  }
}
