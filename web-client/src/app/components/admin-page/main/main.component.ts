import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ShortStatisticsModel } from 'src/app/interfaces/admin-interfaces/short-statistics-model';
import { AdminService } from 'src/app/core/services/admin/admin.service';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss']
})
export class MainComponent implements OnInit {
  shortStatistics: ShortStatisticsModel = {};
  constructor(private router: Router, private adminService: AdminService) {
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  ngOnInit(): void {
    this.adminService.getShortStatistics().subscribe(
      response => {
        this.shortStatistics = response;
        console.log(this.shortStatistics);
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
  }
}
