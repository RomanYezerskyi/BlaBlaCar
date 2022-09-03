import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Chart, registerables } from 'chart.js';
import { AdminStatistics } from 'src/app/interfaces/admin-interfaces/admin-statistics';
import { AdminService } from 'src/app/services/admin/admin.service';
import { ChartService } from 'src/app/services/chart/chart.service';
Chart.register(...registerables);
@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss']
})
export class MainComponent implements OnInit {
  public chart: any;
  public chart1: any;
  public chart2: any;
  public chart3: any;
  statistics: AdminStatistics = {
    usersDateTime: [] = [],
    usersStatisticsCount: [] = [],
    carsDateTime: [] = [],
    carsStatisticsCount: [] = [],
    tripsDateTime: [] = [],
    tripsStatisticsCount: [] = [],
    weekTripsDateTime: [] = [],
    weekStatisticsTripsCount: [] = []
  };
  constructor(private http: HttpClient, private adminService: AdminService, private chartService: ChartService, private router: Router,) {
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  ngOnInit(): void {
    this.getStatistics();


  }
  getStatistics() {
    this.adminService.getStatistics().pipe().subscribe(
      response => {
        this.statistics = response;
        console.log(response)
        console.log(this.statistics.usersStatisticsCount)
        this.createChart();
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
  }
  createChart() {
    // this.chart =
    //   this.chartService.generateChart("myChart", "Users", this.statistics.usersDateTime, this.statistics.usersStatisticsCount);

    // this.chart1 =
    //   this.chartService.generateChart("myChart1", "Cars", this.statistics.carsDateTime, this.statistics.carsStatisticsCount);

    // this.chart2 =
    //   this.chartService.generateChart("myChart2", "Trips", this.statistics.weekTripsDateTime, this.statistics.weekStatisticsTripsCount);

    // this.chart3 =
    //   this.chartService.generateChart("myChart3", "Trips", this.statistics.tripsDateTime, this.statistics.tripsStatisticsCount);

  }

}
