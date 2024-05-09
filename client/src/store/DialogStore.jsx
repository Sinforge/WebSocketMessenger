import { makeAutoObservable} from "mobx"
import { getUserConversation, getUserDialogs } from "../services/message.service";
class DialogStore {
    webSocket = null;
    messages = [];
    dialogs = [];
    openedDialog = null;
    openedUser = null;
    constructor() {
        // makeObservable(this, {
        //     messages: observable,
        //     setMessages: action,
        // })
        makeAutoObservable(this);
    }

    createConnection = (webSocket) => {
        this.webSocket = webSocket;
    }

    updateMessageContent = (message) => {
        if(this.openedDialog === message.senderId) {
            let indexOfMessage = this.messages.findIndex(x => x.id === message.id);
            if(indexOfMessage !== -1) {
                this.messages[indexOfMessage] ={...this.messages[indexOfMessage], content:  message.newContent};
           }
        }
    }

    deleteMessageContent = (messageId) => {
        let indexOfMessage = this.messages.findIndex(x => x.id === messageId);
        if(indexOfMessage !== -1) {
            this.messages.splice(indexOfMessage, 1);
        }
    }
    setSendedMessage = (message) => {
        let indexOfDialog = this.dialogs.findIndex(x => x.id === message.ReceiverId);
        console.log(indexOfDialog)
        if(indexOfDialog !== -1) {
            let element = this.dialogs.splice(indexOfDialog, 1)[0];
            element.lastMessage = message.Content;
            this.dialogs.unshift(element);
        }
        else {
            this.dialogs.push({id : message.SenderId, username: 'income user', lastMessage: message.Content})
        }

        if(this.openedDialog === message.ReceiverId) {
            this.messages.push({
                id: message.Id,
                authorId: message.SenderId,
                content: message.Content,
                messageContentType : message.messageContentType,
                sendTime: message.SendTime
            })
        }
    }
    setIncomeMessage = (message) => {
        let indexOfDialog = this.dialogs.findIndex(x => x.id === message.SenderId);
        if(indexOfDialog !== -1) {
            let element = this.dialogs.splice(indexOfDialog, 1)[0];
            element.lastMessage = message.Content;
            this.dialogs.unshift(element);
        }
        else {
            this.dialogs.push({id : message.SenderId, username: 'income user', lastMessage: message.Content})
        }

        if(this.openedDialog === message.SenderId) {
            this.messages.push({
                id: message.Id,
                authorId: message.SenderId,
                content: message.Content,
                messageContentType : message.messageContentType,
                sendTime: message.SendTime
            })
        }

    }

    setOpenedDialog = (openedUser) => {
        this.openedDialog = openedUser.id;
        this.openedUser = openedUser;
        this.messages = []
    }
    getDialogs = async (axios) => {
        let data = (await getUserDialogs(axios)).data
        this.dialogs = data
    }
    setMessages = async (axios, userId) => {
        this.openedDialog = userId;
        let data = (await getUserConversation(axios, userId)).data
        this.openedUser = this.dialogs.find(x => x.id == userId);

        this.messages = data 
    }

}

export default new DialogStore();