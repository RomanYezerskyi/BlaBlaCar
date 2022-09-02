import { AddAvailableSeats } from "./add-available-seats";

export interface AddTripModel {
    startPlace: string;
    endPlace: string;
    startTime: Date;
    endTime: Date;
    pricePerSeat: number;
    description: string;
    countOfSeats: number;
    carId: number;
    availableSeats: AddAvailableSeats[];
}
