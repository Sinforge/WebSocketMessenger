import React, { useContext, useEffect, useRef, useState } from "react";
import Box from "@mui/material/Box"
import DialogItemList from "./components/Dialog/DialogItemList";
import Divider from "@mui/material/Divider"
import Grid from "@mui/material/Grid"
import MessageList from "./components/MessageList";
import InputField from "./components/InputField";
import { AuthContext } from "../../providers/AuthProvider";
import { w3cwebsocket } from 'websocket';
import DialogHeader from "./components/Dialog/DialogHeader";
import DialogStore from "../../store/DialogStore";
import { toast } from "react-toastify";
import { observer } from "mobx-react-lite";
import FileUploader from './components/FileUploader'
import MessageTypeSwitch from "./components/MessageTypeSwitch";
import GroupItemList from "./components/Group/GroupItemList";
import GroupHeader from "./components/Group/GroupHeader";


const Main = observer(() => {
    const [user, setUser] = useContext(AuthContext);
    //const socket = useRef();

    const socket = useRef(null);
    const {setIncomeMessage, deleteMessageContent, setSendedMessage, updateMessageContent, messageType} = DialogStore;

    console.log(messageType);
    useEffect(() => {
        console.log("open socket")
        socket.current = new w3cwebsocket(
          'ws://localhost:5012?token=' + user.access_token,
          null,
          null,
          null,
          null,
          null,
      );
    
        socket.current.onopen = () => {
            console.log('WebSocket connection established.');
        };
        socket.current.onmessage = (message) => {
            let convertedMessage = JSON.parse(message.data);
            console.log(convertedMessage);
            if(convertedMessage.MessageType === 'CreateMessage') {
            
    
                toast.info("Message from " + convertedMessage.Username + " received ");
                
                setIncomeMessage(convertedMessage)                
            }
            else if(convertedMessage.MessageType === 'UpdateMessage') {
                console.log(convertedMessage);

                toast.dark("Message updated " + convertedMessage.newContent)

                updateMessageContent(convertedMessage);
                
            }

            else if(convertedMessage.MessageType === 'DeleteMessage') {
                console.log(convertedMessage);

                toast.dark("Message deleted");

                deleteMessageContent(convertedMessage.MessageId);
            }
            else if(convertedMessage.MessageType === 'CreatedMessage') {
                console.log(convertedMessage);

                toast.dark("Message successful sended");

                setSendedMessage(convertedMessage)         
            }

        };
    
        return () => {
          socket.current.close();
        };
      }, []);

      

      

    return (

        <Box component='div' sx={{marginTop: "100px"}}>
            <Grid container xs={12}>
                <Grid xs={4}>
                    <MessageTypeSwitch/>
                    { messageType == 0 
                        ? <DialogItemList /> 
                        : <GroupItemList/>}
                </Grid>
                <Grid>
                    <Divider orientation="vertical"/>
                </Grid>
                <Grid xs={7}>
                    { messageType == 0 
                        ? <DialogHeader/>
                        : <GroupHeader/>}
                    <MessageList socket={socket}/>
                    <InputField socket={socket} />
                    <FileUploader socket={socket}/>


                </Grid>
            </Grid>
            

        </Box>
    );
})

export default Main;