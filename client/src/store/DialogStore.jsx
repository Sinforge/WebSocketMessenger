import { makeAutoObservable} from "mobx"
import { getUserConversation, getUserDialogs, getUserGroups, getUserGroupMessages } from "../services/message.service";
class DialogStore {
    webSocket = null;
    messages = [];
    dialogs = [];
    openedDialog = null;
    openedUser = null;
    openedGroup = null;
    messageType = 0;
    groupRoleId = -1;
    constructor() {
        // makeObservable(this, {
        //     messages: observable,
        //     setMessages: action,
        // })
        makeAutoObservable(this);
    }

    setGroupRole = (groupRoleId) => {
        this.groupRoleId = groupRoleId;
    }

    createConnection = (webSocket) => {
        this.webSocket = webSocket;
    }

    deleteGroup = (groupId) => {

        this.dialogs = this.dialogs.filter(x => x.id !== groupId);
    }

    setMessageType = (type) => {
        this.messageType = type;
        this.messages = [];

        this.openedDialog = null;
        this.openedUser = null;
        this.openedGroup = null;

    }

    updateMessageContent = (message) => {
            if(this.openedDialog === message.convId) {
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
        else if(message.messageContentType === this.messageType){
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
        
        let indexOfDialog = null;
        if(this.messageType === 0) indexOfDialog =this.dialogs.findIndex(x => x.id === message.SenderId);
        else indexOfDialog = this.dialogs.findIndex(x => x.id === message.ReceiverId);
        if(indexOfDialog !== -1) {
            let element = this.dialogs.splice(indexOfDialog, 1)[0];
            element.lastMessage = message.Content;
            this.dialogs.unshift(element);
        }
        else {
            this.dialogs.push({id : message.SenderId, username: 'income user', lastMessage: message.Content})
        }

        if(this.messageType === 0) {
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
        else {
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

    }

    setOpenedDialog = (openedUser) => {
        this.openedDialog = openedUser.id;
        this.openedUser = openedUser;
        this.messages = []
    }

    setGroupMessages = async (axios, groupId) => {
        this.openedDialog = groupId;
        let data = (await getUserGroupMessages(axios, groupId)).data
        this.openedGroup = this.dialogs.find(x => x.id == groupId);
        this.messages = data 
    }

    updateGroupLocaly = (groupId, name) => {
        this.dialogs = this.dialogs.map(x => {
            if(x.id === groupId) {
                return {...x, "name":  name}
            }
            return x;
            }
        )
        this.openedGroup = {... this.openedGroup, "name": name}
    }

    addGroup = (group) => {
        const newGroup = {
            id: group.id,
            name: group.name,
            lastMessage: group.lastMessage,
            sendTime: group.sendTime
        };
        this.dialogs.push(newGroup);
    }


    getDialogs = async (axios) => {
        let data = (await getUserDialogs(axios)).data
        this.dialogs = data
    }

    getGroups = async(axios) => {
        let data = (await getUserGroups(axios)).data
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