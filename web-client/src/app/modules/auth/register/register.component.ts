import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormGroupDirective, NgForm, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { RegisterModel } from 'src/app/interfaces/register-model';
import { AuthService } from 'src/app/core/services/auth-service/auth-service.service';
import { PasswordValidatorService } from 'src/app/core/services/password-validator/password-validator.service';
import { AlertsComponent } from '../../shared/components/alerts/alerts.component';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit, OnDestroy {
  @ViewChild(AlertsComponent)
  private alertsComponent: AlertsComponent = new AlertsComponent;
  private unsubscribe$: Subject<void> = new Subject<void>();
  credentials: RegisterModel = {} as RegisterModel;
  form: FormGroup = new FormGroup({});
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
    // this.alertsComponent.showError("aa");
  }
  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
  get formData() {
    return this.form.controls;
  }
  register(form: FormGroupDirective): void {

    if (form.valid) {
      this.authService.Register(this.credentials).pipe(takeUntil(this.unsubscribe$)).subscribe(
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

