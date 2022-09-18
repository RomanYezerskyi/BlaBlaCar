import { MessageStatus } from "src/app/enums/message-status";
import { UserModel } from "../user-interfaces/user-model";
import { ChatModel } from "./chat-model";

export interface MessageModel {
    id: string
    text?: string,
    chatId?: string,
    chat?: ChatModel,
    userId: string,
    user: UserModel,
    createdAt?: Date;
    status?: MessageStatus;
}
