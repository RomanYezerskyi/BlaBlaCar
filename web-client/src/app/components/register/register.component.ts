import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormGroupDirective, NgForm, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthenticatedResponse } from 'src/app/interfaces/authenticated-response';
import { RegisterModel } from 'src/app/interfaces/register';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  invalidLogin: boolean | undefined;
  credentials: RegisterModel = { firstName: '', email: '', phoneNum: '', password: '' };
  data: any;
  emailFormControl = new FormControl('', [Validators.required, Validators.email]);
  itemFormControl = new FormControl('', [Validators.required]);
  passwordFormControl = new FormControl('', [Validators.required]);

  form: FormGroup = new FormGroup({});
  constructor(private router: Router, private http: HttpClient, private fb: FormBuilder) {
    this.form = fb.group({
      password: ['', [Validators.required]],
      confirm_password: ['', [Validators.required]],
      userName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.required]],
    }, {
      validator: this.ConfirmedPasswordValidator('password', 'confirm_password')
    });
  }

  ngOnInit(): void {

  }
  ConfirmedPasswordValidator(controlName: string, matchingControlName: string) {
    return (formGroup: FormGroup) => {
      const control = formGroup.controls[controlName];
      const matchingControl = formGroup.controls[matchingControlName];
      if (matchingControl.errors && !matchingControl.errors['confirmedValidator']) {
        return;
      }
      if (control.value !== matchingControl.value) {
        console.log("aa");
        // this.form.controls['confirm_password'].setErrors({ confirmedValidator: true });
        matchingControl.setErrors({ confirmedValidator: true });
      } else {
        console.log("bb");
        matchingControl.setErrors(null);
      }
    }
  }
  get f() {
    return this.form.controls;
  }
  register = (form: FormGroupDirective) => {
    if (form.valid) {
      this.http.post<AuthenticatedResponse>("https://localhost:5001/api/auth/register", this.credentials, {
        headers: new HttpHeaders({ "Content-Type": "application/json" })
      })
        .subscribe((res) => {
          this.data = res
          console.log(this.data)
          console.error(res);
        })
    }
  }

}

