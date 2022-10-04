import { Component, OnInit } from '@angular/core';
import { PlaceSuggestion } from '../../maps-autocomplete/maps-autocomplete.component';
// // import 'ol/ol.css';
// import Map from 'ol/Map';
// import View from 'ol/View';
// import OSM from 'ol/source/OSM'
// import TileLayer from 'ol/layer/Tile';
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
  autocompleteChanged(value: PlaceSuggestion) {
    console.log(value);

  }
}
