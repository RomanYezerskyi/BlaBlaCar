import { AddAvailableSeatsModel } from "./add-available-seats-model";

export interface AddTripModel {
    startPlace?: string;
    endPlace?: string;
    startTime?: Date;
    endTime?: Date;
    pricePerSeat?: number;
    description?: string;
    countOfSeats?: number;
    carId?: number;
    availableSeats: AddAvailableSeatsModel[];
}
