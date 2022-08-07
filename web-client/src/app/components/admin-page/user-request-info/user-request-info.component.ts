import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CarStatus } from 'src/app/interfaces/car-status';
import { UserModel } from 'src/app/interfaces/user-model';
import { UserStatus } from 'src/app/interfaces/user-status';

@Component({
  selector: 'app-user-request-info',
  templateUrl: './user-request-info.component.html',
  styleUrls: ['./user-request-info.component.scss']
})
export class UserRequestInfoComponent implements OnInit {
  userStatus = UserStatus;
  carStatus = CarStatus;
  userId = '';
  user: UserModel = {
    id: '', drivingLicense: '', email: '', firstName: '', phoneNumber: '',
    roles: [], userStatus: UserStatus.WithoutCar, cars: []
  };
  constructor(private route: ActivatedRoute, private http: HttpClient,) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.userId = params['id'];
    });
    this.searchData();
  }
  searchData = () => {
    const url = 'https://localhost:6001/api/Admin/';
    this.http.get(url + this.userId)
      .subscribe({
        next: (res: any) => {
          this.user = res;
          console.log(this.user);
        },
        error: (err: HttpErrorResponse) => console.error(err),
      });
  }

  convertImg(path: string) {
    return "https://localhost:6001" + path;
  }

  changeUserStatus = (status: UserStatus) => {
    const newStatus = {
      status: status,
      userId: this.user.id
    };
    const url = 'https://localhost:6001/api/Admin/user/status';
    this.http.post(url, newStatus)
      .subscribe({
        next: (res: any) => {
          this.user = res;
          console.log(this.user);
        },
        error: (err: HttpErrorResponse) => console.error(err),
      });
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
