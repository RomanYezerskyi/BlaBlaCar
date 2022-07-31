export interface TripModel {
    id: number;
    startPlace: string;
    endPlace: string;
    startTime: Date;
    endTime: Date;
    pricePerSeat: number;
    description: string;
    countOfSeats: number;
    userId:string;
    
}
