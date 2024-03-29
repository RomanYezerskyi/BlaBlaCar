import { HttpErrorResponse } from '@angular/common/http';
import { Component, Inject, Input, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormGroupDirective, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subject, takeUntil } from 'rxjs';
import { UserModel } from 'src/app/core/models/user-models/user-model';
import { PasswordValidatorService } from 'src/app/core/services/password-validator/password-validator.service';
import { UserService } from 'src/app/core/services/user-service/user.service';

@Component({
  selector: 'app-edit-modal-dialog',
  templateUrl: './edit-modal-dialog.component.html',
  styleUrls: ['./edit-modal-dialog.component.scss']
})
export class EditModalDialogComponent implements OnInit, OnDestroy {
  private unsubscribe$: Subject<void> = new Subject<void>();
  page: number = 1;
  user: UserModel = {} as UserModel;
  onSubmitReason: any;
  private formData = new FormData();
  userDataForm: FormGroup = new FormGroup({});
  passwordForm: FormGroup = new FormGroup({});
  newPasswordModel = { userId: '', currentPassword: '', newPassword: '' };
  constructor(
    private _snackBar: MatSnackBar,
    private dialogRef: MatDialogRef<EditModalDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: any,
    private fb: FormBuilder,
    private userService: UserService,
    private validator: PasswordValidatorService,) {
    this.user = data.user;
    this.userDataForm = fb.group({
      userName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.required]],
    }, {
      validator: null
    });
    this.passwordForm = fb.group({
      password: ['', [Validators.required]],
      newPassword: ['', [Validators.required]],
      confirm_password: ['', [Validators.required]],
    }, {
      validator: this.validator.ConfirmedPasswordValidator('newPassword', 'confirm_password')  // this.ConfirmedPasswordValidator('password', 'confirm_password')
    });
  }

  ngOnInit(): void {
  }
  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
  get getUserForm() {
    return this.userDataForm.controls;
  }
  get getPasswordForm() {
    return this.passwordForm.controls;
  }
  changePage(page: number) {
    this.page = page;
  }
  close() {
    this.dialogRef.close();
  }
  updateUserPassword(form: FormGroupDirective): void {
    this.newPasswordModel.userId = this.user.id;
    this.userService.updateUserPassword(this.newPasswordModel).pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        console.log(response)
        this.openSnackBar("Password has been changed!");
      },
      (error: HttpErrorResponse) => { console.log(error.error); this.openSnackBar(error.error); }
    );
  }
  updateUser(form: FormGroupDirective) {
    if (!form.valid) return;
    let userModel = {
      id: this.user.id,
      email: this.user.email,
      firstName: this.user.firstName,
      phoneNumber: this.user.phoneNumber
    }
    this.userService.updateUserInfo(userModel).pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        this.getUser();
        this.openSnackBar("Changes saved");
      },
      (error: HttpErrorResponse) => { console.log(error.error); this.openSnackBar(error.error.error); }
    );
  }
  private openSnackBar(message: string) {
    this._snackBar.open(message, "Close");
  }
  getUser(): void {
    this.userService.getCurrentUser().pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        this.userService.userProfile = response;
      },
      (error: HttpErrorResponse) => { console.log(error.error); }
    );;
  }
}
