export interface Message{
    id:string
    senderId:string
    senderUsername:string
    senderFirstName:string
    senderLastName:string
    receiverId:string
    sentAt:string
    message:string
    isRead:boolean
    chatGroupId:string
}