import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormGroupDirective, NgForm, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthenticatedResponse } from 'src/app/interfaces/authenticated-response';
import { RegisterModel } from 'src/app/interfaces/register';
import { AuthService } from 'src/app/services/authservice/auth-service.service';
import { PasswordValidatorService } from 'src/app/services/password-validator/password-validator.service';
import { AlertsComponent } from '../alerts/alerts.component';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  credentials: RegisterModel = { firstName: '', email: '', phoneNum: '', password: '' };
  data: any;
  emailFormControl = new FormControl('', [Validators.required, Validators.email]);
  itemFormControl = new FormControl('', [Validators.required]);
  passwordFormControl = new FormControl('', [Validators.required]);
  form: FormGroup = new FormGroup({});
  @ViewChild(AlertsComponent)
  private alertsComponent: AlertsComponent = new AlertsComponent;
  constructor(private router: Router, private http: HttpClient, private fb: FormBuilder,
    private validator: PasswordValidatorService, private authService: AuthService) {
    this.form = fb.group({
      password: ['', [Validators.required]],
      confirm_password: ['', [Validators.required]],
      userName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.required]],
    }, {
      validator: this.validator.ConfirmedPasswordValidator('password', 'confirm_password')  // this.ConfirmedPasswordValidator('password', 'confirm_password')
    });
  }

  ngOnInit(): void {
    this.alertsComponent.showError("aa");
  }
  get f() {
    return this.form.controls;
  }
  register = (form: FormGroupDirective) => {

    if (form.valid) {
      this.authService.Register(this.credentials).pipe().subscribe(
        response => {
          console.log(response);

        },
        (error: HttpErrorResponse) => {
          console.log(error.error);
          this.alertsComponent.showError("Something went wrong! Try again!");
        }
      )
    }
  }

}

