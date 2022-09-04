import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { CarType } from 'src/app/enums/car-type';
import { CarStatus } from 'src/app/interfaces/car-interfaces/car-status';
import { UserModel } from 'src/app/interfaces/user-interfaces/user-model';
import { UserStatus } from 'src/app/interfaces/user-interfaces/user-status';
import { AdminService } from 'src/app/services/admin/admin.service';
import { ImgSanitizerService } from 'src/app/services/imgsanitizer/img-sanitizer.service';
import { UserService } from 'src/app/services/userservice/user.service';

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
export class UserRequestInfoComponent implements OnInit, OnDestroy, OnChanges {
  userStatus = UserStatus;
  carStatus = CarStatus;
  carType = CarType;
  selectedUser: UserModel = {
    id: '', cars: [], email: '', firstName: '', phoneNumber: '', roles: [], userDocuments: [], userStatus: -1,
  };
  usersSubscription!: Subscription;
  constructor(private userService: UserService,
    private sanitizeImgService: ImgSanitizerService,
    private route: ActivatedRoute,
    private http: HttpClient, private adminService: AdminService) { }
  @Input() userId = '';
  ngOnInit(): void {
    if (this.userId == '') {
      this.route.params.subscribe(params => {
        this.userId = params['id'];
      });
    }
    this.getUser(this.userId);
  }
  ngOnDestroy(): void {
    this.usersSubscription.unsubscribe();
  }
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['userId'] && changes['userId']?.previousValue != changes['userId']?.currentValue) {
      if (this.userId != '') this.getUser(this.userId)
    }
  }
  sanitizeImg(img: string): SafeUrl {
    return this.sanitizeImgService.sanitiizeUserImg(img);
  }
  getUser(userId: string) {
    this.usersSubscription = this.userService.getUserFromApi(userId).subscribe(
      response => {
        console.log(response);
        this.selectedUser = response;
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
  }
  changeUserStatus = (status: UserStatus) => {
    if (this.selectedUser.userStatus == status) return;
    const newStatus = {
      status: status,
      userId: this.selectedUser.id
    };
    this.adminService.changeUserDrivingLicenseStatus(newStatus).pipe().subscribe(
      response => {
        window.alert(response);
        console.log(response);
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
  }
  changeCarStatus = (status: CarStatus, carId: number) => {
    if (this.selectedUser.cars.find(x => x.id == carId)?.carStatus == status) return;
    const newStatus = {
      status: status,
      carId: carId,
    };
    this.adminService.changeCarDocumentsStatus(newStatus).pipe().subscribe(
      response => {
        console.log(response);
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );

  }

}
