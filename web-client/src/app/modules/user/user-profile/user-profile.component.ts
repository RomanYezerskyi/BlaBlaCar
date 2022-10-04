import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormGroupDirective, Validators } from '@angular/forms';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { SafeUrl } from '@angular/platform-browser';
import { Chart, registerables } from 'chart.js';
import { Subject, takeUntil } from 'rxjs';
import { UserModel } from 'src/app/interfaces/user-interfaces/user-model';
import { CarService } from 'src/app/core/services/car-service/car.service';
import { ImgSanitizerService } from 'src/app/core/services/image-sanitizer-service/img-sanitizer.service';
import { PasswordValidatorService } from 'src/app/core/services/password-validator/password-validator.service';
import { UserService } from 'src/app/core/services/user-service/user.service';
import { RequestDrivingLicenseComponent } from './request-driving-license/request-driving-license.component';
import { EditModalDialogComponent } from './edit-modal-dialog/edit-modal-dialog.component';
Chart.register(...registerables);
@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit, OnDestroy {
  private unsubscribe$: Subject<void> = new Subject<void>();
  user: UserModel = {} as UserModel;
  imagePath: any;
  changeUserPhoto = false;
  message: string | undefined;
  private formData = new FormData();
  constructor(
    private dialog: MatDialog,
    private imgSanitaze: ImgSanitizerService,
    private userService: UserService) {
  }

  ngOnInit(): void {
    this.getUser();
  }
  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
  editData(): void {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.data = {
      user: this.user
    }
    const dRef = this.dialog.open(EditModalDialogComponent, dialogConfig);
    // dRef.componentInstance.onSubmitReason.subscribe(() => {
    //   this.searchData();
    // });
  }
  editDocuments(): void {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.data = {
      user: this.user
    }
    const dRef = this.dialog.open(RequestDrivingLicenseComponent, dialogConfig);
    // dRef.componentInstance.onSubmitReason.subscribe(() => {
    //   this.searchData();
    // });
  }
  changePhoto(): void {
    this.changeUserPhoto = !this.changeUserPhoto;
    if (this.imagePath != null) this.imagePath = null;
  }
  updateUserPhoto() {
    this.userService.updateUserPhoto(this.formData).pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        this.ngOnInit();
        console.log(response)
      },
      (error: HttpErrorResponse) => { console.log(error.error); }
    );
  }

  onFileChanged(event: any): void {
    const file = event.target.files[0];
    if (file.type.match(/image\/*/) == null) {
      this.message = "Only images are supported.";
      return;
    }
    this.formData.append('userImg', file, file.name);
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = (_event) => {
      this.imagePath = reader.result;
    }
  }
  sanitizeUserImg(img: string): SafeUrl {
    return this.imgSanitaze.sanitiizeUserImg(img);
  }
  getUser(): void {
    this.userService.getCurrentUser().pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        this.user = response;
        console.log(response)
      },
      (error: HttpErrorResponse) => { console.log(error.error); }
    );
  }

}
