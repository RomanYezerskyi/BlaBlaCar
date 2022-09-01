import { Component, OnInit } from '@angular/core';
import { Chart, registerables } from 'chart.js';
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
  constructor() {

  }

  ngOnInit(): void {
    this.createChart();
  }
  createChart() {

    this.chart = new Chart("myChart", {
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
    this.chart1 = new Chart("myChart1", {
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
    this.chart2 = new Chart("myChart2", {
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
    this.chart3 = new Chart("myChart3", {
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
  }

}
