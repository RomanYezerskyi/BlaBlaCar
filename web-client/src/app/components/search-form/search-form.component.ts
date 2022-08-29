import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SearchTripModel } from 'src/app/interfaces/search-trip';

@Component({
  selector: 'app-search-form',
  templateUrl: './search-form.component.html',
  styleUrls: ['./search-form.component.scss']
})
export class SearchFormComponent implements OnInit {
  counter_active = false;
  @Input() trip: SearchTripModel = {
    countOfSeats: 1,
    endPlace: '',
    startPlace: '',
    startTime: new Date(),
  };

  constructor(private router: Router,) { }

  ngOnInit(): void {
  }
  counterAcive() {
    this.counter_active = !this.counter_active;
  }
  counterAdd() {
    if (this.trip.countOfSeats == 8) return;
    this.trip.countOfSeats += 1;
  }
  counterRemove() {
    if (this.trip.countOfSeats == 1) return;
    this.trip.countOfSeats -= 1;
  }
  search() {
    this.router.navigate(['search'], {
      queryParams: {
        startPlace: this.trip.startPlace,
        endPlace: this.trip.endPlace,
        startTime: this.trip.startTime.toLocaleDateString(),
        seats: this.trip.countOfSeats,
      }
    });
  }

}


