import { Injectable } from '@angular/core';
import { Chart, registerables } from 'chart.js';
Chart.register(...registerables);
@Injectable({
  providedIn: 'root'
})
export class ChartService {

  constructor() { }

  generateChart(chartId: string, label: string, xArray: Array<Date | string>, yArray: Array<any>): Chart {
    return new Chart(chartId, {
      type: 'line', //this denotes tha type of chart

      data: {// values on X-Axis
        // labels: ['2022-05-10', '2022-05-11', '2022-05-12', '2022-05-13',
        //   '2022-05-14', '2022-05-15', '2022-05-16', '2022-05-17',],
        labels: xArray,
        datasets: [
          {
            label: label,
            data: yArray,
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
              display: true
            },
            grid: {
              display: false
            }

          },


          yAxis: {
            ticks:
            {
              display: true
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
