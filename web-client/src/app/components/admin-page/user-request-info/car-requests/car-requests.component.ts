import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { CarStatus } from 'src/app/interfaces/car-status';
import { UserModel } from 'src/app/interfaces/user-model';
import { UserStatus } from 'src/app/interfaces/user-status';

@Component({
  selector: 'app-car-requests',
  templateUrl: './car-requests.component.html',
  styleUrls: ['./car-requests.component.scss', '../user-request-info.component.scss']
})
export class CarRequestsComponent implements OnInit {
  userStatus = UserStatus;
  carStatus = CarStatus;
  userId = '';
  @Input() user: UserModel = {
    id: '', userDocuments: [], email: '', firstName: '', phoneNumber: '',
    roles: [], userStatus: UserStatus.WithoutCar, cars: [] = []
  };

  constructor(private http: HttpClient, private sanitizer: DomSanitizer) { }

  ngOnInit(): void {
  }
  sanitaizeImg(img: string): SafeUrl {
    let file = 'https://localhost:6001/' + img;
    console.log(file);
    return this.sanitizer.bypassSecurityTrustUrl(file);
  }
  changeCarStatus = (status: CarStatus, carId: number) => {
    const newStatus = {
      status: status,
      carId: carId,
    };
    const url = 'https://localhost:6001/api/Admin/car/status';
    this.http.post(url, newStatus)
      .subscribe({
        next: (res: any) => {
          this.user = res;
          console.log(this.user);
        },
        error: (err: HttpErrorResponse) => console.error(err),
      });
  }
}
