import { CarType } from "../enums/car-type";
import { CarStatus } from "./car-status";
import { SeatModel } from "./seat";

export interface CarModel {
    id: number;
    modelName: string;
    registNum:string;
    carType: CarType;
    seats: SeatModel[];
    techPassport: string;
    carStatus:CarStatus;
}
