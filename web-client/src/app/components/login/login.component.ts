import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LoginModel } from '../../interfaces/login';
import { AuthenticatedResponse } from '../../interfaces/authenticated-response';
import { FormControl, NgForm, Validators } from '@angular/forms';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  invalidLogin: boolean | undefined;
  credentials: LoginModel = { email: '', password: '' };
  emailFormControl = new FormControl('', [Validators.required, Validators.email]);
  passwordFormControl = new FormControl('', [Validators.required]);
  constructor(private router: Router, private http: HttpClient) { }

  ngOnInit(): void {
  }

  login = (form: NgForm) => {
    if (form.valid) {
      this.http.post<AuthenticatedResponse>("https://localhost:5001/api/auth/login", this.credentials, {
        headers: new HttpHeaders({ "Content-Type": "application/json" })
      })
        .subscribe({
          next: (response: AuthenticatedResponse) => {
            const token = response.token;
            const refreshToken = response.refreshToken;
            localStorage.setItem("jwt", token);
            localStorage.setItem("refreshToken", refreshToken);
            this.invalidLogin = false;
            this.router.navigate(["/"]);
          },
          error: (err: HttpErrorResponse) => this.invalidLogin = true
        })
    }
  }

}
