import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component, Inject, Input, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { UserModel } from 'src/app/interfaces/user-model';
import { UserStatus } from 'src/app/interfaces/user-status';

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
  newPassword = { userId: '', currentPassword: '', newPassword: '' }
  private formData = new FormData();
  @Input() user: UserModel = {
    id: '', email: '', firstName: '', phoneNumber: '',
    roles: [], cars: [] = [], userDocuments: [] = [], userStatus: -1, trips: [], tripUsers: []
  };
  constructor(private http: HttpClient, private router: Router, private sanitizer: DomSanitizer,
    private dialogRef: MatDialogRef<UserProfileComponent>, @Inject(MAT_DIALOG_DATA) data: any,) { }

  async ngOnInit() {
    this.user = await this.getUser();
    this.dialogRef.updateSize('60%', '80%');
  }
  editData() {
    this.editUserData = !this.editUserData;

  }
  changePhoto() {
    this.changeUserPhoto = !this.changeUserPhoto;
    if (this.imagePath != null) this.imagePath = null;
  }
  async updateUserPhoto(): Promise<any> {
    const updateUser = new Promise<HttpHeaders>((resolve, reject) => {
      this.http.post('https://localhost:6001/api/User/updateUserImg', this.formData)
        .subscribe({
          next: (res: any) => {
            resolve(res);
            this.ngOnInit();
            console.log(res);
          },
          error: (err: HttpErrorResponse) => { console.log(err); reject(err) }
        });
    })
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
  async updateUser(form: NgForm): Promise<any> {
    if (!form.valid) return;
    const res = await new Promise<HttpHeaders>((resolve, reject) => {
      let userModel = {
        id: this.user.id,
        email: this.user.email,
        firstName: this.user.firstName,
        phoneNumber: this.user.phoneNumber
      }
      this.http.post("https://localhost:6001/api/User/update", userModel).subscribe({
        next: (res: any) => { resolve(res); console.log(res) },
        error: (_) => {
          reject(_);
        }
      });
    });
    return res;
  }
  async getUser(): Promise<UserModel> {
    const url = 'https://localhost:6001/api/User';
    const user = await new Promise<UserModel>((resolve, reject) => {
      this.http.get<UserModel>(url)
        .subscribe({
          next: (res: UserModel) => {
            resolve(res);
            console.log(res);
          },
          error: (err: HttpErrorResponse) => { console.error(err); reject(err) },
        });
    })
    return user;
  }
}
