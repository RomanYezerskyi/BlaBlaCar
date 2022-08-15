import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
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
  styleUrls: ['./user-request-info.component.scss']
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
  page = 0;
  constructor(private route: ActivatedRoute, private http: HttpClient,
    private sanitizer: DomSanitizer) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.userId = params['id'];
    });
    this.getUser();
    // this.route.queryParams.subscribe(params => {
    //   this.menu = params['type'];
    // });

  }
  getOutPut(event: any) {
    this.ngOnInit(); // or something that you can use to make it
  }
  check = 1;
  changePage(item: Menu) {
    this.check = item as number;
    console.log(this.check);
  }

  sanitaizeImg(img: string): SafeUrl {
    let file = 'https://localhost:6001/' + img;
    console.log(file);
    return this.sanitizer.bypassSecurityTrustUrl(file);
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
