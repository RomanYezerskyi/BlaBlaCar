import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { CarStatus } from 'src/app/interfaces/car-interfaces/car-status';
import { UserModel } from 'src/app/interfaces/user-interfaces/user-model';
import { UserStatus } from 'src/app/interfaces/user-interfaces/user-status';
import { AdminService } from 'src/app/services/admin/admin.service';

@Component({
  selector: 'app-main-info',
  templateUrl: './main-info.component.html',
  styleUrls: ['./main-info.component.scss', '../user-request-info.component.scss']
})
export class MainInfoComponent implements OnInit {
  userStatus = UserStatus;
  carStatus = CarStatus;
  userId = '';
  @Input() user: UserModel = {
    id: '', userDocuments: [], email: '', firstName: '', phoneNumber: '',
    roles: [], userStatus: UserStatus.WithoutCar, cars: [] = []
  };

  constructor(private http: HttpClient, private sanitizer: DomSanitizer, private adminService: AdminService) { }

  ngOnInit(): void {
  }
  sanitaizeImg(img: string): SafeUrl {
    return this.sanitizer.bypassSecurityTrustUrl(img);
  }
  changeUserStatus = (status: UserStatus) => {
    const newStatus = {
      status: status,
      userId: this.user.id
    };
    this.adminService.changeUserDrivingLicenseStatus(newStatus).pipe().subscribe(
      response => {
        window.alert(response);
        console.log(response);
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
    // const url = 'https://localhost:6001/api/Admin/user/status';
    // this.http.post(url, newStatus)
    //   .subscribe({
    //     next: (res: any) => {
    //       window.alert(res);
    //       console.log(res);
    //     },
    //     error: (err: HttpErrorResponse) => console.error(err),
    //   });
  }
}
