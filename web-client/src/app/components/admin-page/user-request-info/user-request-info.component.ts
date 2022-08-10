import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { CarStatus } from 'src/app/interfaces/car-status';
import { UserModel } from 'src/app/interfaces/user-model';
import { UserStatus } from 'src/app/interfaces/user-status';

export enum Menu {
  User = 1,
  Licenses = 2,
  Cars = 3
}

@Component({
  selector: 'app-user-request-info',
  templateUrl: './user-request-info.component.html',
  styleUrls: ['./user-request-info.component.scss'],

})
export class UserRequestInfoComponent implements OnInit {
  menu = Menu;
  userStatus = UserStatus;
  carStatus = CarStatus;
  userId = '';
  user: UserModel = {
    id: '', userDocuments: [], email: '', firstName: '', phoneNumber: '',
    roles: [], userStatus: UserStatus.WithoutCar, cars: [] = []
  };
  private formData = new FormData();
  img = 'data:image/jpeg;base64,';
  result = '';
  constructor(private route: ActivatedRoute, private http: HttpClient,
    private sanitizer: DomSanitizer) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.userId = params['id'];
    });
    this.getUser();

  }

  check = Menu.User;
  changePage(item: Menu) {
    this.check = item;
    console.log(this.check);
  }

  sanitaizeImg(img: string): SafeUrl {
    let file = 'https://localhost:6001/' + img;
    console.log(file);
    return this.sanitizer.bypassSecurityTrustUrl(file);
  }

  getImg = (img: string) => {
    this.formData.append("img", img);
    const url = 'https://localhost:6001/api/DriverFiles/';
    this.http.post(url, this.formData)
      .subscribe({
        next: (res: any) => {
          console.log(res);
          // this.img = res.fileContents;
          this.result = this.img + res.fileContents;
          return this.result;
          //console.log(this.result);
        },
        error: (err: HttpErrorResponse) => console.error(err),
      });
  }


  getUser = () => {
    const url = 'https://localhost:6001/api/Admin/';
    this.http.get(url + this.userId)
      .subscribe({
        next: (res: any) => {
          this.user = res as UserModel;
          console.log(this.user);
        },
        error: (err: HttpErrorResponse) => console.error(err),
      });
  }
}
