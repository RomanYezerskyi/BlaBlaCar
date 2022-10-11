import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormGroupDirective, NgForm, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { RegisterModel } from 'src/app/core/models/auth-models/register-model';
import { AuthService } from 'src/app/core/services/auth-service/auth-service.service';
import { PasswordValidatorService } from 'src/app/core/services/password-validator/password-validator.service';
import { environment } from 'src/environments/environment';
import { AlertsComponent } from '../../shared/components/alerts/alerts.component';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit, OnDestroy {
  @ViewChild(AlertsComponent)
  private alertsComponent: AlertsComponent = new AlertsComponent;
  @ViewChild('registerFormDiv') registerForm!: ElementRef;
  @ViewChild('login') login!: ElementRef;
  private unsubscribe$: Subject<void> = new Subject<void>();
  credentials: RegisterModel = {} as RegisterModel;
  form!: FormGroup;
  constructor(private validator: PasswordValidatorService, private authService: AuthService) {
  }

  ngOnInit(): void {

    this.form = new FormGroup({
      password: new FormControl(['', [Validators.required]]),
      confirmPassword: new FormControl(['']),
      userName: new FormControl(['', [Validators.required]]),
      email: new FormControl(['', [Validators.required, Validators.email]]),
      phoneNumber: new FormControl(['', [Validators.required]]),
    });
    this.form.get('confirmPassword')!.setValidators([Validators.required,
    this.validator.validateConfirmPassword(this.form.get('password')!)!]);

  }
  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
  public validateControl = (controlName: string) => {
    return this.form.get(controlName)?.invalid && this.form.get(controlName)?.touched;
  }
  public hasError = (controlName: string, errorName: string) => {
    return this.form.get(controlName)?.hasError(errorName);
  }
  register(form: FormGroupDirective): void {
    this.credentials.clientURI = environment.emailConfirmation;
    if (form.valid) {
      this.authService.Register(this.credentials).pipe(takeUntil(this.unsubscribe$)).subscribe(
        response => {
          this.registerForm.nativeElement.style.display = 'none';
          this.login.nativeElement.style.margin = 'auto';
          this.alertsComponent.showSuccessMessages("You have successfully registered!\n Now go to your email and verify it", 10000);
        },
        (error: HttpErrorResponse) => {
          console.log(error.error);
          this.alertsComponent.showError(error.error, 10000);
        }
      )
    }
  }

}

