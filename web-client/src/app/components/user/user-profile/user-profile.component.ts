import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component, Inject, Input, OnInit } from '@angular/core';
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
  @Input() user: UserModel = {
    id: '', email: '', firstName: '', phoneNumber: '',
    roles: [], cars: [] = [], userDocuments: [] = [], userStatus: -1, trips: [], tripUsers: []
  };
  constructor(private http: HttpClient, private router: Router, private sanitizer: DomSanitizer,
    private dialogRef: MatDialogRef<UserProfileComponent>, @Inject(MAT_DIALOG_DATA) data: any,) { }

  ngOnInit(): void {
    this.getUser();
    this.dialogRef.updateSize('60%', '80%');
  }
  sanitaizeImg(img: string): SafeUrl {
    return this.sanitizer.bypassSecurityTrustUrl(img);
  }
  close() {
    this.dialogRef.close();
  }
  async changeUserName(): Promise<HttpHeaders> {
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
  getUser = () => {
    const url = 'https://localhost:6001/api/User';
    this.http.get(url)
      .subscribe({
        next: (res: any) => {
          this.user = res as UserModel;
          console.log(res);
        },
        error: (err: HttpErrorResponse) => console.error(err),
      });

  }
}
