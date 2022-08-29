import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { CarModel } from 'src/app/interfaces/car';
import { CarStatus } from 'src/app/interfaces/car-status';
import { UserModel } from 'src/app/interfaces/user-model';
import { CarService } from 'src/app/services/carservice/car.service';
import { UserService } from 'src/app/services/userservice/user.service';

@Component({
  selector: 'app-user-cars',
  templateUrl: './user-cars.component.html',
  styleUrls: ['./user-cars.component.scss']
})
export class UserCarsComponent implements OnInit {
  cars: CarModel[] = [];
  carStatus = CarStatus;
  constructor(private http: HttpClient, private router: Router, private sanitizer: DomSanitizer,
    private userService: UserService, private carService: CarService) { }

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
}
