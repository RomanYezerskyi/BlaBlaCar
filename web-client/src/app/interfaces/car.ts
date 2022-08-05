import { CarType } from "../enums/car-type";
import { SeatModel } from "./seat";

export interface CarModel {
    id: number;
    modelName: string;
    registNum:string;
    carType: CarType;
    seats: SeatModel[];
}
