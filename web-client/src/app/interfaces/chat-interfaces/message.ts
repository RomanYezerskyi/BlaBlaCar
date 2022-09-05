import { UserModel } from "../user-interfaces/user-model";
import { Chat } from "./chat";

export interface Message {
    text?: string,
    chatId?: string,
    chat?: Chat,
    userId: string,
    user: UserModel,
    createdAt?: Date;
}
