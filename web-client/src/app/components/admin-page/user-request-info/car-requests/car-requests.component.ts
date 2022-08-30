import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { AddNewCarModel } from 'src/app/interfaces/addnew-car';
import { CarStatus } from 'src/app/interfaces/car-status';
import { UserModel } from 'src/app/interfaces/user-model';
import { UserStatus } from 'src/app/interfaces/user-status';
import { AdminService } from 'src/app/services/admin/admin.service';

@Component({
  selector: 'app-car-requests',
  templateUrl: './car-requests.component.html',
  styleUrls: ['./car-requests.component.scss', '../user-request-info.component.scss'],
})
export class CarRequestsComponent implements OnInit {
  userStatus = UserStatus;
  carStatus = CarStatus;
  userId = '';
  @Input() user: UserModel = {
    id: '', userDocuments: [], email: '', firstName: '', phoneNumber: '',
    roles: [], userStatus: UserStatus.WithoutCar, cars: [] = []
  };
  @Output() someOutput: EventEmitter<any> = new EventEmitter<any>;
  constructor(private http: HttpClient, private sanitizer: DomSanitizer, private adminService: AdminService) { }

  ngOnInit(): void {

  }
  sanitaizeImg(img: string): SafeUrl {
    return this.sanitizer.bypassSecurityTrustUrl(img);
  }
  changeCarStatus = (status: CarStatus, carId: number) => {
    const newStatus = {
      status: status,
      carId: carId,
    };
    this.adminService.changeCarDocumentsStatus(newStatus).pipe().subscribe(
      response => {
        this.someOutput.emit("");
        console.log(response);
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
    // const url = 'https://localhost:6001/api/Admin/car/status';
    // this.http.post(url, newStatus)
    //   .subscribe({
    //     next: (res: any) => {
    //       this.someOutput.emit("");
    //     },
    //     error: (err: HttpErrorResponse) => console.error(err),
    //   });
  }
}
