import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TripModel } from 'src/app/interfaces/trip-interfaces/trip-model';
import { GeocodingFeatureProperties } from 'src/app/interfaces/autocomplete-interfaces/place-suggestion-model';

@Injectable({
  providedIn: 'root'
})
export class MapsService {

  constructor(private http: HttpClient) { }

  getPlaceName(text: string): Observable<any> {
    const url = `https://api.geoapify.com/v1/geocode/search?text=${text}&apiKey=32849d6b3d3b480c9a60be1ce5891252`;
    return this.http.get<any>(url);
  }
  getPlace(text: string): Observable<any> {
    const url = `https://api.geoapify.com/v1/geocode/autocomplete?text=${text}&limit=5&apiKey=d06cb6573e1e488d92494d5296611f0c`;
    return this.http.get<any>(url);
  }
  generateShortAddress(properties: GeocodingFeatureProperties): string {
    let shortAddress = properties.name;

    if (!shortAddress && properties.street && properties.housenumber) {
      // name is not set for buildings
      shortAddress = `${properties.street} ${properties.housenumber}`;
    }

    shortAddress += (properties.postcode && properties.city) ? `, ${properties.postcode}-${properties.city}` : '';
    shortAddress += (!properties.postcode && properties.city && properties.city !== properties.name) ? `, ${properties.city}` : '';
    shortAddress += (properties.country && properties.country !== properties.name) ? `, ${properties.country}` : '';

    return shortAddress;
  }
  generateFullAddress(properties: GeocodingFeatureProperties): string {
    let fullAddress = properties.name ? properties.name : properties.address_line1;
    fullAddress += properties.street ? `, ${properties.street}` : '';
    fullAddress += properties.housenumber ? ` ${properties.housenumber}` : '';
    fullAddress += (properties.postcode && properties.city) ? `, ${properties.postcode}-${properties.city}` : '';
    fullAddress += (!properties.postcode && properties.city && properties.city !== properties.name) ? `, ${properties.city}` : '';
    fullAddress += properties.state ? `, ${properties.state}` : '';
    fullAddress += (properties.country && properties.country !== properties.name) ? `, ${properties.country}` : '';
    return fullAddress;
  }
}
