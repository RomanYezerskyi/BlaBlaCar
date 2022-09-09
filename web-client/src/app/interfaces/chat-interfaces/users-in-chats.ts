import { UserModel } from "../user-interfaces/user-model";
import { Chat } from "./chat";

export interface UsersInChats {
    userId?: string,
    user?: UserModel,
    chatId?: string,
    chat?: Chat,
    role: number
}
