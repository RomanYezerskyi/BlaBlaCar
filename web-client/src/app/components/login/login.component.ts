import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { LoginModel } from '../../interfaces/login';
import { AuthenticatedResponse } from '../../interfaces/authenticated-response';
import { FormControl, NgForm, Validators } from '@angular/forms';
import { AuthService } from 'src/app/services/authservice/auth-service.service';
import { first } from 'rxjs';
import { AlertsComponent } from '../alerts/alerts.component';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  credentials: LoginModel = { email: '', password: '' };
  emailFormControl = new FormControl('', [Validators.required, Validators.email]);
  passwordFormControl = new FormControl('', [Validators.required]);
  returnUrl: string | undefined;
  @ViewChild(AlertsComponent)
  private alertsComponent: AlertsComponent = new AlertsComponent;
  constructor(private router: Router,
    private http: HttpClient,
    private route: ActivatedRoute,
    private authService: AuthService) { }

  ngOnInit(): void {
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  login = (form: NgForm) => {
    if (form.valid) {
      this.authService.login(this.credentials)
        .pipe().subscribe(
          response => {
            this.router.navigateByUrl(this.returnUrl!);

          },
          (error: HttpErrorResponse) => { this.alertsComponent.showError(error.error) }
        )
    }
  }

}
