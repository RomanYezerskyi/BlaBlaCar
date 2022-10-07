import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';

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
}
