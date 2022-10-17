import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ResetPasswordModel } from 'src/app/core/models/auth-models/reset-password-model';
import { AuthService } from 'src/app/core/services/auth-service/auth-service.service';
import { PasswordValidatorService } from 'src/app/core/services/password-validator/password-validator.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {
  resetPasswordForm!: FormGroup;
  showSuccess!: boolean;
  showError!: boolean;
  errorMessage!: string;
  private token!: string;
  private email!: string;

  constructor(private authService: AuthService, private passConfValidator: PasswordValidatorService,
    private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.resetPasswordForm = new FormGroup({
      password: new FormControl('', [Validators.required]),
      confirmPassword: new FormControl('')
    },);

    this.resetPasswordForm.get('confirmPassword')!.setValidators([Validators.required,
    this.passConfValidator.validateConfirmPassword(this.resetPasswordForm.get('password')!)!]);

    this.token = this.route.snapshot.queryParams['token'];
    this.email = this.route.snapshot.queryParams['email'];
  }
  public validateControl = (controlName: string) => {
    return this.resetPasswordForm.get(controlName)?.invalid && this.resetPasswordForm.get(controlName)?.touched
  }
  public hasError = (controlName: string, errorName: string) => {
    return this.resetPasswordForm.get(controlName)?.hasError(errorName)
  }
  public resetPassword = (resetPasswordFormValue: ResetPasswordModel) => {
    this.showError = this.showSuccess = false;
    const resetPass = { ...resetPasswordFormValue };
    const resetPassModel: ResetPasswordModel = {
      password: resetPass.password,
      confirmPassword: resetPass.confirmPassword,
      token: this.token,
      email: this.email
    }
    console.log(resetPassModel);
    this.authService.resetPassword(resetPassModel)
      .subscribe({
        next: (_) => this.showSuccess = true,
        error: (err: HttpErrorResponse) => {
          this.showError = true;
          this.errorMessage = err.error;
        }
      })
  }

}
