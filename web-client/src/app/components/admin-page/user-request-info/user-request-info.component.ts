import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { CarStatus } from 'src/app/interfaces/car-interfaces/car-status';
import { UserModel } from 'src/app/interfaces/user-interfaces/user-model';
import { UserStatus } from 'src/app/interfaces/user-interfaces/user-status';
import { AdminService } from 'src/app/services/admin/admin.service';

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
    private sanitizer: DomSanitizer, private adminService: AdminService) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.userId = params['id'];
    });
    this.getUserRequest();
    // this.route.queryParams.subscribe(params => {
    //   this.menu = params['type'];
    // });

  }
  getOutPut(event: any) {
    this.ngOnInit();
  }
  check = 1;
  changePage(item: Menu) {
    this.check = item as number;
    console.log(this.check);
  }

  sanitaizeImg(img: string): SafeUrl {
    return this.sanitizer.bypassSecurityTrustUrl(img);
  }


  getUserRequest = () => {
    this.adminService.getUserRequest(this.userId).pipe().subscribe(
      response => {
        this.user = response;
        console.log(response);
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
    // const url = 'https://localhost:6001/api/Admin/';
    // this.http.get(url + this.userId)
    //   .subscribe({
    //     next: (res: any) => {
    //       this.user = res as UserModel;
    //       console.log(this.user);
    //     },
    //     error: (err: HttpErrorResponse) => console.error(err),
    //   });
  }
}
