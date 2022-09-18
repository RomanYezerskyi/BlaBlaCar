import { SeatModel } from "../car-interfaces/seat-model";

export interface TripUserModel {
    id: string,
    seat: SeatModel,
    seatId: string;
}
