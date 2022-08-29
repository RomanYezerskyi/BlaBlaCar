export interface SearchTripModel {
    startPlace: string;
    endPlace: string;
    startTime: Date | string;
    countOfSeats: number;
    skip?: number;
    take?: number;
}
