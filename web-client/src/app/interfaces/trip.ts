import { AvailableSeatsModel } from "./available-seats";
import { CarModel } from "./car";
import { UserModel } from "./user-model";

export interface TripModel {
    id: number;
    startPlace: string;
    endPlace: string;
    startTime: Date;
    endTime: Date;
    pricePerSeat: number;
    description: string;
    countOfSeats: number;
    userId: string;
    availableSeats: AvailableSeatsModel[];
    car: CarModel
    user?: UserModel;
}
