import { CarType } from "../../enums/car-type";
import { CarDocuments } from "./car-documents";
import { CarStatus } from "./car-status";
import { SeatModel } from "./seat";
import { TripModel } from "../trip-interfaces/trip";

export interface CarModel {
    id: number;
    modelName: string;
    registNum: string;
    carType: CarType;
    seats: SeatModel[];
    carDocuments: CarDocuments[]
    carStatus: CarStatus;
    trips?: TripModel[],
    createdAt?: Date
}
