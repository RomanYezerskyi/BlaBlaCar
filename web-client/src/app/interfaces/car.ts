import { CarType } from "../enums/car-type";

export interface CarModel {
    id: number;
    modelName: string;
    registNum:string;
    carType: CarType;
}
