import { SeatModel } from "../car-models/seat-model";

export interface TripUserModel {
    id: string,
    seat: SeatModel,
    seatId: string;
}
