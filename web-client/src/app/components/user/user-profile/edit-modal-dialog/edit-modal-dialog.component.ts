import { HttpErrorResponse } from '@angular/common/http';
import { Component, Inject, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormGroupDirective, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { UserModel } from 'src/app/interfaces/user-interfaces/user-model';
import { UserService } from 'src/app/services/userservice/user.service';

@Component({
  selector: 'app-edit-modal-dialog',
  templateUrl: './edit-modal-dialog.component.html',
  styleUrls: ['./edit-modal-dialog.component.scss']
})
export class EditModalDialogComponent implements OnInit {
  user: UserModel = {} as UserModel;
  onSubmitReason: any;
  private formData = new FormData();
  userDataForm: FormGroup = new FormGroup({});
  constructor(
    private dialogRef: MatDialogRef<EditModalDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: any,
    private fb: FormBuilder,
    private userService: UserService) {
    this.user = data.user;
    this.userDataForm = fb.group({
      userName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.required]],
    }, {
      validator: null
    });
  }

  ngOnInit(): void {
  }
  get getUserForm() {
    return this.userDataForm.controls;
  }
  close() {
    this.dialogRef.close();
  }
  updateUser(form: FormGroupDirective) {
    if (!form.valid) return;
    let userModel = {
      id: this.user.id,
      email: this.user.email,
      firstName: this.user.firstName,
      phoneNumber: this.user.phoneNumber
    }
    this.userService.updateUserInfo(userModel).pipe().subscribe(
      response => {
        console.log(response)
      },
      (error: HttpErrorResponse) => { console.log(error.error); }
    );
  }
}
