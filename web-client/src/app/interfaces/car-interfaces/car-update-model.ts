import { CarType } from "src/app/enums/car-type";
import { CarDocuments } from "./car-documents";
import { SeatModel } from "./seat";

export interface CarUpdateModel {
    id: number;
    modelName: string;
    registNum: string;
    countOfSeats: number;
    carType: CarType;
    seats: Array<SeatModel>;
    techPassportFile: File[];
    deletedDocuments: CarDocuments[];
}
