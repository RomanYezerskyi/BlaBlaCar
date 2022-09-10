import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { CarModel } from 'src/app/interfaces/car-interfaces/car';
import { CarStatus } from 'src/app/interfaces/car-interfaces/car-status';
import { UserModel } from 'src/app/interfaces/user-interfaces/user-model';
import { CarService } from 'src/app/services/carservice/car.service';
import { UserService } from 'src/app/services/userservice/user.service';
import { EditCarModalDialogComponent } from './edit-car-modal-dialog/edit-car-modal-dialog.component';

@Component({
  selector: 'app-user-cars',
  templateUrl: './user-cars.component.html',
  styleUrls: ['./user-cars.component.scss']
})
export class UserCarsComponent implements OnInit {
  cars: CarModel[] = [];
  carStatus = CarStatus;
  constructor(private http: HttpClient, private router: Router, private sanitizer: DomSanitizer,
    private userService: UserService, private carService: CarService, private dialog: MatDialog,) { }

  ngOnInit(): void {
    this.getUserCars()
  }
  sanitaizeImg(img: string): SafeUrl {
    return this.sanitizer.bypassSecurityTrustUrl(img);
  }
  getUserCars() {
    this.carService.getUserCars().pipe().subscribe(
      response => {
        this.cars = response;
        console.log(response)
      },
      (error: HttpErrorResponse) => { console.log(error); }
    );
  }
  edit(carId: number) {
    const dialogConfig = new MatDialogConfig();

    // dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.data = {
      car: this.cars.find(x => x.id == carId)
    }
    const dRef = this.dialog.open(EditCarModalDialogComponent, dialogConfig);

    // dRef.componentInstance.onSubmitReason.subscribe(() => {
    //   this.searchData();
    // });
  }
}
