import { SeatModel } from "../car-interfaces/seat-model";

export interface BookedTripModel {
    id: number;
    tripId: number;
    requestedSeats: number;
    bookedSeats: SeatModel[];
}
