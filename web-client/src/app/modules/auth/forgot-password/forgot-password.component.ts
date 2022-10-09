import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ForgotPasswordModel } from 'src/app/core/models/auth-models/forgot-password-model';
import { AuthService } from 'src/app/core/services/auth-service/auth-service.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent implements OnInit {
  forgotPasswordForm!: FormGroup
  successMessage!: string;
  errorMessage!: string;
  showSuccess!: boolean;
  showError!: boolean;

  constructor(private _authService: AuthService) { }

  ngOnInit(): void {
    this.forgotPasswordForm = new FormGroup({
      email: new FormControl("", [Validators.required])
    })
  }
  validateControl = (controlName: string) => {
    return this.forgotPasswordForm.get(controlName)?.invalid && this.forgotPasswordForm.get(controlName)?.touched
  }
  hasError = (controlName: string, errorName: string) => {
    return this.forgotPasswordForm.get(controlName)?.hasError(errorName)
  }
  forgotPassword = (forgotPasswordFormValue: ForgotPasswordModel) => {
    this.showError = this.showSuccess = false;
    const forgotPass = { ...forgotPasswordFormValue };
    const forgotPassDto: ForgotPasswordModel = {
      email: forgotPass.email,
      clientURI: environment.resetPasswordUrl,
    }
    this._authService.forgotPassword(forgotPassDto)
      .subscribe({
        next: () => {
          this.showSuccess = true;
          this.successMessage = 'The link has been sent, please check your email to reset your password.'
        },
        error: (err: HttpErrorResponse) => {
          this.showError = true;
          this.errorMessage = err.error;
          ;
        }
      })
  }

}
