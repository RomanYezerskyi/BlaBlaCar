import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormControl, NgForm, Validators } from '@angular/forms';
import { AuthService } from 'src/app/core/services/auth-service/auth-service.service';
import { first, Subject, takeUntil } from 'rxjs';
import { AlertsComponent } from '../../shared/components/alerts/alerts.component';
import { LoginModel } from 'src/app/interfaces/login-model';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit, OnDestroy {
  @ViewChild(AlertsComponent)
  private alertsComponent: AlertsComponent = new AlertsComponent;
  private unsubscribe$: Subject<void> = new Subject<void>();
  credentials: LoginModel = { email: '', password: '' };
  emailFormControl = new FormControl('', [Validators.required, Validators.email]);
  passwordFormControl = new FormControl('', [Validators.required]);
  returnUrl: string | undefined;
  constructor(private router: Router,
    private route: ActivatedRoute,
    private authService: AuthService) { }

  ngOnInit(): void {
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }
  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  login(form: NgForm): void {
    if (form.valid) {
      this.authService.login(this.credentials)
        .pipe(takeUntil(this.unsubscribe$)).subscribe(
          response => {
            this.router.navigateByUrl(this.returnUrl!);
          },
          (error: HttpErrorResponse) => { this.alertsComponent.showError(error.error) }
        )
    }
  }

}
