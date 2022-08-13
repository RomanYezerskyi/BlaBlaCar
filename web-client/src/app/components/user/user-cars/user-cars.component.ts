import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { CarStatus } from 'src/app/interfaces/car-status';
import { UserModel } from 'src/app/interfaces/user-model';

@Component({
  selector: 'app-user-cars',
  templateUrl: './user-cars.component.html',
  styleUrls: ['./user-cars.component.scss']
})
export class UserCarsComponent implements OnInit {
  @Input() user: UserModel = {
    id: '', email: '', firstName: '', phoneNumber: '',
    roles: [], cars: [] = [], userDocuments: [] = [], userStatus: -1, trips: [], tripUsers: []
  };
  carStatus = CarStatus;
  constructor(private http: HttpClient, private router: Router, private sanitizer: DomSanitizer) { }

  ngOnInit(): void {
    this.getUser();
  }
  sanitaizeImg(img: string): SafeUrl {
    let file = 'https://localhost:6001/' + img;
    console.log(file);
    return this.sanitizer.bypassSecurityTrustUrl(file);
  }
  getUser = () => {
    const url = 'https://localhost:6001/api/User';
    this.http.get(url)
      .subscribe({
        next: (res: any) => {
          this.user = res as UserModel;
          console.log(res);
        },
        error: (err: HttpErrorResponse) => console.error(err),
      });

  }
}
