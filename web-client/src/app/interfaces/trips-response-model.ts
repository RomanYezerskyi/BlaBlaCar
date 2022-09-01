import { AvailableSeatsModel } from "./available-seats";
import { BookedTripUsers } from "./booked-trip-users";
import { CarModel } from "./car";
import { TripModel } from "./trip";
import { UserModel } from "./user-model";

export interface TripsResponseModel {
    trips: TripModel[],
    totalTrips: number
}