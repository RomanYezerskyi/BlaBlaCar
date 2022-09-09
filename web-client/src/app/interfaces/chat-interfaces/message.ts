import { MessageStatus } from "src/app/enums/message-status";
import { UserModel } from "../user-interfaces/user-model";
import { Chat } from "./chat";

export interface Message {
    id: string
    text?: string,
    chatId?: string,
    chat?: Chat,
    userId: string,
    user: UserModel,
    createdAt?: Date;
    status?: MessageStatus;
}
