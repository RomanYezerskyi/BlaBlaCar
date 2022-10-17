import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Observable, Subject, Subscription, takeUntil } from 'rxjs';
import { CarType } from 'src/app/core/enums/car-type';
import { NotificationStatus } from 'src/app/core/enums/notification-status';
import { CarDocumentsModel } from 'src/app/core/models/car-models/car-documents-model';
import { CarStatus } from 'src/app/core/models/car-models/car-status';
import { UserModel } from 'src/app/core/models/user-models/user-model';
import { UserStatus } from 'src/app/core/models/user-models/user-status';
import { AdminService } from 'src/app/core/services/admin/admin.service';
import { ImgSanitizerService } from 'src/app/core/services/image-sanitizer-service/img-sanitizer.service';
import { NotificationsService } from 'src/app/core/services/notifications-service/notifications.service';
import { UserService } from 'src/app/core/services/user-service/user.service';
import { CreateNotificationDialogComponent } from '../../shared/components/create-notification-dialog/create-notification-dialog.component';

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
  selectedUser: UserModel = {
    cars: [] = [],
    email: '',
    firstName: '',
    id: '',
    phoneNumber: '',
    roles: [] = [], userDocuments: [] = [], userStatus: 0, trips: [] = [], tripUsers: [] = [],
  };
  isSpinner: boolean = false;
  constructor(private userService: UserService,
    private sanitizeImgService: ImgSanitizerService,
    private route: ActivatedRoute,
    private adminService: AdminService,
    private dialog: MatDialog,
    private _snackBar: MatSnackBar,) { }

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
    dialogConfig.autoFocus = true;
    dialogConfig.data = {
      notificationStatus: notificationStatus,
      userId: userId,
      title: "Explain what is wrong with the user documents"
    };
    const dRef = this.dialog.open(CreateNotificationDialogComponent, dialogConfig);
  }
  sanitizeImg(img: string): SafeUrl {
    return this.sanitizeImgService.sanitiizeUserImg(img);
  }
  getUser(userId: string) {
    this.isSpinner = true;
    this.userService.getUserFromApi(userId).pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        this.selectedUser = response;
        this.selectedUser.userDocuments.forEach(x => this.userDocs.push(x.drivingLicense));
        this.isSpinner = false;
      },
      (error: HttpErrorResponse) => { console.error(error.error); this.isSpinner = false; }
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
        this.openSnackBar("User status changed!");
        this.getUser(this.userId);
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
        this.openSnackBar("User status changed!");
        this.getUser(this.userId);
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );

  }
  getCarDoc(carDocuments: CarDocumentsModel[]): Array<string> {
    let images: Array<string> = [];
    carDocuments.forEach(x => images.push(x.technicalPassport));
    return images;
  }
  private openSnackBar(message: string) {
    this._snackBar.open(message, "Close");
  }

}
