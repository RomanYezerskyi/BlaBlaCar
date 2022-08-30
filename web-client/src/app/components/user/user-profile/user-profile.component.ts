import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component, Inject, Input, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormGroupDirective, NgForm, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { UserModel } from 'src/app/interfaces/user-model';
import { UserStatus } from 'src/app/interfaces/user-status';
import { PasswordValidatorService } from 'src/app/services/password-validator/password-validator.service';
import { UserService } from 'src/app/services/userservice/user.service';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit {
  editUserData = false;
  imagePath: any;
  changeUserPhoto = false;
  message: string | undefined;

  private formData = new FormData();
  emailFormControl = new FormControl('', [Validators.required, Validators.email]);
  userFormControl = new FormControl('', [Validators.required]);
  @Input() user: UserModel = {
    id: '', email: '', firstName: '', phoneNumber: '',
    roles: [], cars: [] = [], userDocuments: [] = [], userStatus: -1, trips: [], tripUsers: []
  };
  newPasswordModel = { userId: '', currentPassword: '', newPassword: '' };
  userDataForm: FormGroup = new FormGroup({});
  form: FormGroup = new FormGroup({});
  constructor(private http: HttpClient, private router: Router, private sanitizer: DomSanitizer,
    private dialogRef: MatDialogRef<UserProfileComponent>, @Inject(MAT_DIALOG_DATA) data: any, private fb: FormBuilder,
    private validator: PasswordValidatorService, private userService: UserService) {
    this.userDataForm = fb.group({
      userName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.required]],
    }, {
      validator: null
    });
    this.form = fb.group({
      password: ['', [Validators.required]],
      newPassword: ['', [Validators.required]],
      confirm_password: ['', [Validators.required]],
    }, {
      validator: this.validator.ConfirmedPasswordValidator('newPassword', 'confirm_password')  // this.ConfirmedPasswordValidator('password', 'confirm_password')
    });
  }

  async ngOnInit() {
    this.getUser();
    this.dialogRef.updateSize('60%', '80%');
  }
  get getUserForm() {
    return this.userDataForm.controls;
  }
  get f() {
    return this.form.controls;
  }
  editData() {
    this.editUserData = !this.editUserData;

  }
  changePhoto() {
    this.changeUserPhoto = !this.changeUserPhoto;
    if (this.imagePath != null) this.imagePath = null;
  }
  updateUserPhoto() {
    this.userService.updateUserPhoto(this.formData).pipe().subscribe(
      response => {
        this.ngOnInit();
        console.log(response)
      },
      (error: HttpErrorResponse) => { console.log(error.error); }
    );
    // const updateUser = new Promise<HttpHeaders>((resolve, reject) => {
    //   this.http.post('https://localhost:6001/api/User/updateUserImg', this.formData)
    //     .subscribe({
    //       next: (res: any) => {
    //         resolve(res);
    //         this.ngOnInit();
    //         console.log(res);
    //       },
    //       error: (err: HttpErrorResponse) => { console.log(err); reject(err) }
    //     });
    // })
  }

  onFileChanged(event: any) {

    const file = event.target.files[0];
    if (file.type.match(/image\/*/) == null) {
      this.message = "Only images are supported.";
      return;
    }
    this.formData.append('userImg', file, file.name);
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = (_event) => {
      this.imagePath = reader.result;
    }
  }
  sanitaizeImg(img: string): SafeUrl {
    return this.sanitizer.bypassSecurityTrustUrl(img);
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
    // const res = await new Promise<HttpHeaders>((resolve, reject) => {

    //   this.http.post("https://localhost:6001/api/User/update", userModel).subscribe({
    //     next: (res: any) => { resolve(res); console.log(res) },
    //     error: (_) => {
    //       reject(_);
    //     }
    //   });
    // });
    // return res;
  }
  async updateUserPassword(form: FormGroupDirective): Promise<any> {
    if (!form.valid) return;
    this.newPasswordModel.userId = this.user.id;
    this.userService.updateUserPassword(this.newPasswordModel).pipe().subscribe(
      response => {
        console.log(response)
      },
      (error: HttpErrorResponse) => { console.log(error.error); }
    );
    // const res = await new Promise<HttpHeaders>((resolve, reject) => {
    //   this.http.post("https://localhost:5001/api/User/update-password", this.newPasswordModel).subscribe({
    //     next: (res: any) => { resolve(res); console.log(res) },
    //     error: (_) => {
    //       reject(_);
    //     }
    //   });
    // });
    // return res;
  }
  getUser() {
    this.userService.getUser().pipe().subscribe(
      response => {
        this.user = response;
        console.log(response)
      },
      (error: HttpErrorResponse) => { console.log(error.error); }
    );
    // const url = 'https://localhost:6001/api/User';
    // const user = await new Promise<UserModel>((resolve, reject) => {
    //   this.http.get<UserModel>(url)
    //     .subscribe({
    //       next: (res: UserModel) => {
    //         resolve(res);
    //         console.log(res);
    //       },
    //       error: (err: HttpErrorResponse) => { console.error(err); reject(err) },
    //     });
    // })
    // return user;
  }
}
