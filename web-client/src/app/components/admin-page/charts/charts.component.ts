import { Component, OnDestroy, OnInit } from '@angular/core';
import { AdminStatisticsModel } from 'src/app/interfaces/admin-interfaces/admin-statistics-model';
import { Chart, registerables } from 'chart.js';
import { ChartService } from 'src/app/services/chart/chart.service';
import { AdminService } from 'src/app/services/admin/admin.service';
import { HttpErrorResponse } from '@angular/common/http';
import { Subject, takeUntil } from 'rxjs';
Chart.register(...registerables);
@Component({
  selector: 'app-charts',
  templateUrl: './charts.component.html',
  styleUrls: ['./charts.component.scss']
})
export class ChartsComponent implements OnInit, OnDestroy {
  private unsubscribe$: Subject<void> = new Subject<void>();
  public chart?: Chart;
  public chart1?: Chart;
  public chart2?: Chart;
  public chart3?: Chart;
  statistics: AdminStatisticsModel = {
    usersDateTime: [] = [],
    usersStatisticsCount: [] = [],
    carsDateTime: [] = [],
    carsStatisticsCount: [] = [],
    tripsDateTime: [] = [],
    tripsStatisticsCount: [] = [],
    weekTripsDateTime: [] = [],
    weekStatisticsTripsCount: [] = []
  };
  searchDate = new Date();
  constructor(private chartService: ChartService, private adminService: AdminService) { }

  ngOnInit(): void {
    this.getStatistics();
  }
  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  searchByDate(): void {
    this.chart!.destroy();
    this.chart1!.destroy();
    this.chart2!.destroy();
    this.chart3!.destroy();
    this.getStatistics();
  }
  getStatistics(): void {
    this.adminService.getStatistics(this.searchDate.toDateString()).pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        this.statistics = response;
        console.log(response)
        console.log(this.statistics.usersStatisticsCount)
        this.createChart();
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
  }
  createChart(): void {
    this.chart =
      this.chartService.generateChart("myChart", "Users", this.statistics.usersDateTime, this.statistics.usersStatisticsCount);

    this.chart1 =
      this.chartService.generateChart("myChart1", "Cars", this.statistics.carsDateTime, this.statistics.carsStatisticsCount);

    this.chart2 =
      this.chartService.generateChart("myChart2", "Trips", this.statistics.weekTripsDateTime, this.statistics.weekStatisticsTripsCount);

    this.chart3 =
      this.chartService.generateChart("myChart3", "Trips", this.statistics.tripsDateTime, this.statistics.tripsStatisticsCount);
  }

}
