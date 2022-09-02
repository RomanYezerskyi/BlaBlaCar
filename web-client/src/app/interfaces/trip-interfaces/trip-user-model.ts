import { SeatModel } from "../car-interfaces/seat";

export interface TripUserModel {
    id: string,
    seat: SeatModel,
    seatId: string;
}
