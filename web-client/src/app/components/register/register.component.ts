import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, NgForm, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthenticatedResponse } from 'src/app/interfaces/authenticated-response';
import { RegisterModel } from 'src/app/interfaces/register';
import { PasswordValidation } from './password-validation';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  invalidLogin: boolean | undefined;
  credentials: RegisterModel = { firstName: '', email: '', phoneNum: '', password: '' };
  data: any;
  form: FormGroup | undefined;
  emailFormControl = new FormControl('', [Validators.required, Validators.email]);
  itemFormControl = new FormControl('', [Validators.required]);
  passwordFormControl = new FormControl('', [Validators.required]);
  constructor(private router: Router, private http: HttpClient, private formBuilder: FormBuilder,) {

  }

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      password: ['', Validators.required],
      confirmPassword: ['', Validators.required],

    }, {
      validator: PasswordValidation.MatchPassword
    });
  }
  register = (form: NgForm) => {
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

