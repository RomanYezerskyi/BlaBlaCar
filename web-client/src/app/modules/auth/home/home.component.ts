import { Component, OnInit } from '@angular/core';
import { PlaceSuggestionModel } from 'src/app/core/models/autocomplete-models/place-suggestion-model';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  // public map!: Map;
  constructor() { }

  ngOnInit(): void {
    // this.map = new Map({
    //   layers: [
    //     new TileLayer({
    //       source: new OSM(),
    //     }),
    //   ],
    //   target: 'map',
    //   view: new View({
    //     center: [0, 0],
    //     zoom: 2, maxZoom: 18,
    //   }),
    // });

  }
  autocompleteChanged(value: PlaceSuggestionModel) {
    console.log(value);

  }
}
