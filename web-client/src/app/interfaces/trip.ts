import { AvailableSeatsModel } from "./available-seats";
import { CarModel } from "./car";

export interface TripModel {
    id: number;
    startPlace: string;
    endPlace: string;
    startTime: Date;
    endTime: Date;
    pricePerSeat: number;
    description: string;
    countOfSeats: number;
    userId:string;
    availableSeats: AvailableSeatsModel[]; 
    car: CarModel
}
