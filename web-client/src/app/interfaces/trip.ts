import { AvailableSeatsModel } from "./available-seats";
import { BookedTripUsers } from "./booked-trip-users";
import { CarModel } from "./car";
import { TripUserModel } from "./trip-user-model";
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
    tripUsers: any[];
    bookedTripUsers?: BookedTripUsers[];
}
