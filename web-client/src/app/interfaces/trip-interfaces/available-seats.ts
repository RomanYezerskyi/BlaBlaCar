import { AvailableSeatsType } from "../../enums/available-seats-type";
export interface AvailableSeatsModel {
    id: number,
    tripId: number,
    seatId: number,
    availableSeatsType: AvailableSeatsType,
    seat: any
}
