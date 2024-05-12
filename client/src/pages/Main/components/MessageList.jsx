import React, { useContext } from "react";
import Message from "./Messages/Message";
import Box from "@mui/material/Box"
import DialogStore from "../../../store/DialogStore";
import { observer } from "mobx-react-lite"
import { jwtDecode } from "jwt-decode";
import { AuthContext } from "../../../providers/AuthProvider";
import { getMessageFile } from "../../../services/message.service";
import FileMessage from "./Messages/FileMessage";
import useAxiosPrivateWithBlob from "../../../hooks/useAxiosPrivateWithBlob";

const MessageList = observer(({socket}) => {
    const { messages, deleteMessageContent, openedDialog, messageType ,updateMessageContent} = DialogStore;
    const [user, setUser] = useContext(AuthContext);
    const axios = useAxiosPrivateWithBlob();
    const myId = jwtDecode(user.access_token)["Id"];

    const updateMessage = (messageId, newContent) => {
        if (socket.current && socket.current.readyState === socket.current.OPEN) {
            let messageObj = {
                "HeaderInfo": {
                    "Method" : "UpdateMessage",
                    "To": openedDialog,
                    "Type": messageType + 1
                },
                "MessageContent": {
                    "MessageId": messageId,
                    "NewContent": newContent
                }
            }
            socket.current.send(JSON.stringify(messageObj));
          } else {
            console.error('WebSocket not open to send message');
          }
    }

    const downloadFile = async (messageId, messageName) => {
        
        getMessageFile(axios, messageId).then(response => {
            const url = window.URL.createObjectURL(new Blob([response.data]));
            const link = document.createElement('a');
            link.href = url;
            link.setAttribute('download', messageName); // Set the filename here
            document.body.appendChild(link);
            link.click();
            // Clean up
            link.parentNode.removeChild(link);
            window.URL.revokeObjectURL(url);
          })
          .catch(error => {
            console.error('Error downloading file:', error);
          });
    }
    const deleteMessage = (messageId) => {
        if (socket.current && socket.current.readyState === socket.current.OPEN) {
            let messageObj = {
                "HeaderInfo": {
                    "Method" : "DeleteMessage",
                    "To": openedDialog,
                    "Type": 1
            
                },
                "MessageContent": {
                    "MessageId": messageId
                }
            }
            socket.current.send(JSON.stringify(messageObj));
            deleteMessageContent(messageId);
          } else {
            console.error('WebSocket not open to send message');
          }
    }
    
    return(
        <Box sx={{height: '600px', overflowY: 'auto'}} >
            {
                messages.map((message, i) => {
                    if(message.messageContentType == 1) {
                        return <Message key={i} myId={myId}  message={message}
                updateMessage={updateMessage} deleteMessage={deleteMessage}/>
                    }
                    else {
                        return <FileMessage key={i} myId={myId}  message={message}
                    downloadFile={downloadFile} deleteMessage={deleteMessage}/>
                    }
            })
            }
            
        </Box>
    )
})
export default MessageList;