import { AvailableSeatsModel } from "./available-seats";
import { BookedTripUsers } from "./booked-trip-users";
import { CarModel } from "../car-interfaces/car";
import { TripModel } from "./trip-model";
import { UserModel } from "../user-interfaces/user-model";

export interface TripsResponseModel {
    trips: TripModel[],
    totalTrips: number
}
