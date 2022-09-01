import { Component, Input, OnInit } from '@angular/core';
import { Chart, registerables } from 'chart.js';
Chart.register(...registerables);
@Component({
  selector: 'app-chart',
  templateUrl: './chart.component.html',
  styleUrls: ['./chart.component.scss']
})
export class ChartComponent implements OnInit {
  public chart: any;
  @Input() script: string = "MyChart1";
  constructor() { }

  ngOnInit(): void {
    this.createChart();
  }
  createChart() {

    this.chart = new Chart(this.script, {
      type: 'line', //this denotes tha type of chart

      data: {// values on X-Axis
        labels: ['2022-05-10', '2022-05-11', '2022-05-12', '2022-05-13',
          '2022-05-14', '2022-05-15', '2022-05-16', '2022-05-17',],
        datasets: [
          {
            label: "Sales",
            data: ['467', '576', '572', '79', '92',
              '574', '573', '576'],
            backgroundColor: 'blue'
          },
          // {
          //   label: "Profit",
          //   data: ['542', '542', '536', '327', '17',
          //     '0.00', '538', '541'],
          //   backgroundColor: 'limegreen'
          // }
        ]
      },
      options: {

        scales: {
          xAxis: {
            // The axis for this scale is determined from the first letter of the id as `'x'`
            // It is recommended to specify `position` and / or `axis` explicitly.
            ticks: {
              display: false
            },
            grid: {
              display: false
            }

          },


          yAxis: {
            ticks:
            {
              display: false
            },
            grid: {
              display: false
            }
          }

        },
        aspectRatio: 2.5,
        plugins: {

          legend: {
            display: false,

          },



        },

      }

    });
    console.log(this.chart);
  }

}
