export interface SearchTripModel {
    startPlace: string;
    endPlace: string;
    startTime: Date;
    countOfSeats: number;
    skip?: number;
    take?: number;
}
