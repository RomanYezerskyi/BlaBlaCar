import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Observable, Subject, Subscription, takeUntil } from 'rxjs';
import { CarType } from 'src/app/enums/car-type';
import { NotificationStatus } from 'src/app/enums/notification-status';
import { CarDocumentsModel } from 'src/app/interfaces/car-interfaces/car-documents-model';
import { CarStatus } from 'src/app/interfaces/car-interfaces/car-status';
import { UserModel } from 'src/app/interfaces/user-interfaces/user-model';
import { UserStatus } from 'src/app/interfaces/user-interfaces/user-status';
import { AdminService } from 'src/app/services/admin/admin.service';
import { ImgSanitizerService } from 'src/app/services/imgsanitizer/img-sanitizer.service';
import { NotificationsService } from 'src/app/services/notificationsservice/notifications.service';
import { UserService } from 'src/app/services/userservice/user.service';
import { CreateNotificationDialogComponent } from '../../create-notification-dialog/create-notification-dialog.component';


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
  @Input() userId: string = '';
  private unsubscribe$: Subject<void> = new Subject<void>();
  userDocs: Array<string> = [];
  userStatus = UserStatus;
  carStatus = CarStatus;
  carType = CarType;
  selectedUser: UserModel = {} as UserModel;
  constructor(private userService: UserService,
    private sanitizeImgService: ImgSanitizerService,
    private route: ActivatedRoute,
    private adminService: AdminService,
    private dialog: MatDialog) { }

  ngOnInit(): void {
    if (this.userId == '') {
      this.route.params.subscribe(params => {
        this.userId = params['id'];
      });
    }
    this.getUser(this.userId);
  }
  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['userId'] && changes['userId']?.previousValue != changes['userId']?.currentValue) {
      if (this.userId != '') this.getUser(this.userId)
    }
  }
  addNotitfication(): void {
    this.openNotificationDialog(NotificationStatus.SpecificUser, this.selectedUser.id);
    this.changeUserStatus(this.userStatus.NeedMoreData);
  }
  private openNotificationDialog(notificationStatus: NotificationStatus, userId: string | null): void {
    const dialogConfig = new MatDialogConfig();
    // dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.data = {
      notificationStatus: notificationStatus,
      userId: userId,
      title: "Notification"
    };
    const dRef = this.dialog.open(CreateNotificationDialogComponent, dialogConfig);
    // dRef.componentInstance.onSubmitReason.subscribe(() => {
    //   this.changeUserStatus(this.userStatus.NeedMoreData);
    // });
  }
  sanitizeImg(img: string): SafeUrl {
    return this.sanitizeImgService.sanitiizeUserImg(img);
  }
  getUser(userId: string) {
    this.userService.getUserFromApi(userId).pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        console.log(response);
        this.selectedUser = response;
        this.selectedUser.userDocuments.forEach(x => this.userDocs.push(x.drivingLicense));
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
  }
  changeUserStatus(status: UserStatus): void {
    if (this.selectedUser.userStatus == status) return;
    const newStatus = {
      status: status,
      userId: this.selectedUser.id
    };
    this.adminService.changeUserDrivingLicenseStatus(newStatus).pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        window.alert(response);
        console.log(response);
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
  }
  changeCarStatus(status: CarStatus, carId: number): void {
    if (this.selectedUser.cars.find(x => x.id == carId)?.carStatus == status) return;
    const newStatus = {
      status: status,
      carId: carId,
    };
    this.adminService.changeCarDocumentsStatus(newStatus).pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        console.log(response);
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );

  }
  getCarDoc(carDocuments: CarDocumentsModel[]): Array<string> {
    let images: Array<string> = [];
    carDocuments.forEach(x => images.push(x.techPassport));
    return images;
  }

}
