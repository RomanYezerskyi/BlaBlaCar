import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component, Inject, Input, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormGroupDirective, NgForm, Validators } from '@angular/forms';
import { MatDialog, MatDialogConfig, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { Chart, registerables } from 'chart.js';
import { UserModel } from 'src/app/interfaces/user-interfaces/user-model';
import { UserStatisticsModel } from 'src/app/interfaces/user-interfaces/user-statistics-model';
import { UserStatus } from 'src/app/interfaces/user-interfaces/user-status';
import { ChartService } from 'src/app/services/chart/chart.service';
import { ImgSanitizerService } from 'src/app/services/imgsanitizer/img-sanitizer.service';
import { PasswordValidatorService } from 'src/app/services/password-validator/password-validator.service';
import { UserService } from 'src/app/services/userservice/user.service';
import { EditModalDialogComponent } from './edit-modal-dialog/edit-modal-dialog.component';
Chart.register(...registerables);
@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit {
  user: UserModel = {} as UserModel;
  private statistics: UserStatisticsModel = {} as UserStatisticsModel


  public chart?: Chart;
  public chart1?: Chart;
  editUserData = false;
  imagePath: any;
  changeUserPhoto = false;
  message: string | undefined;

  private formData = new FormData();
  emailFormControl = new FormControl('', [Validators.required, Validators.email]);
  userFormControl = new FormControl('', [Validators.required]);

  newPasswordModel = { userId: '', currentPassword: '', newPassword: '' };
  userDataForm: FormGroup = new FormGroup({});
  form: FormGroup = new FormGroup({});
  constructor(
    private dialog: MatDialog,
    private chartService: ChartService,
    private imgSanitaze: ImgSanitizerService,
    private fb: FormBuilder,
    private validator: PasswordValidatorService,
    private userService: UserService) {
    this.userDataForm = fb.group({
      userName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.required]],
    }, {
      validator: null
    });
    this.form = fb.group({
      password: ['', [Validators.required]],
      newPassword: ['', [Validators.required]],
      confirm_password: ['', [Validators.required]],
    }, {
      validator: this.validator.ConfirmedPasswordValidator('newPassword', 'confirm_password')  // this.ConfirmedPasswordValidator('password', 'confirm_password')
    });
  }

  async ngOnInit() {
    this.getUser();
    // this.getUserStatistic();
  }
  createChart() {

    // this.chart =
    //   this.chartService.generateChart("myChart", "Trips", this.statistics.tripsDateTime, this.statistics.tripsStatisticsCount);

    // this.chart1 =
    //   this.chartService.generateChart("myChart1", "Cars", this.statistics.carsDateTime, this.statistics.carsStatisticsCount);

  }
  // getUserStatistic() {
  //   this.userService.getUserStatistics().pipe().subscribe(
  //     response => {
  //       this.statistics.tripsStatisticsCount = response.tripsStatisticsCount;
  //       this.statistics.tripsDateTime = response.tripsDateTime;
  //       console.log(this.statistics);
  //       this.createChart();
  //     },
  //     (error: HttpErrorResponse) => { console.log(error.error); }
  //   );
  // }


  get getUserForm() {
    return this.userDataForm.controls;
  }
  get f() {
    return this.form.controls;
  }
  editData() {
    const dialogConfig = new MatDialogConfig();

    // dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.data = {
      user: this.user
    }
    const dRef = this.dialog.open(EditModalDialogComponent, dialogConfig);

    // dRef.componentInstance.onSubmitReason.subscribe(() => {
    //   this.searchData();
    // });
  }

  changePhoto() {
    this.changeUserPhoto = !this.changeUserPhoto;
    if (this.imagePath != null) this.imagePath = null;
  }
  updateUserPhoto() {
    this.userService.updateUserPhoto(this.formData).pipe().subscribe(
      response => {
        this.ngOnInit();
        console.log(response)
      },
      (error: HttpErrorResponse) => { console.log(error.error); }
    );
  }

  onFileChanged(event: any) {

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

  updateUser(form: FormGroupDirective) {
    if (!form.valid) return;
    let userModel = {
      id: this.user.id,
      email: this.user.email,
      firstName: this.user.firstName,
      phoneNumber: this.user.phoneNumber
    }
    this.userService.updateUserInfo(userModel).pipe().subscribe(
      response => {
        console.log(response)
      },
      (error: HttpErrorResponse) => { console.log(error.error); }
    );
  }
  updateUserPassword(form: FormGroupDirective): void {
    if (!form.valid) return;
    this.newPasswordModel.userId = this.user.id;
    this.userService.updateUserPassword(this.newPasswordModel).pipe().subscribe(
      response => {
        console.log(response)
      },
      (error: HttpErrorResponse) => { console.log(error.error); }
    );
  }
  getUser() {
    this.userService.getCurrentUser().pipe().subscribe(
      response => {
        this.user = response;
        console.log(response)
      },
      (error: HttpErrorResponse) => { console.log(error.error); }
    );
  }
}
