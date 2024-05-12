import React, { useState } from "react";
import DialogItem from "./DialogItem";
import Box from "@mui/material/Box"
import DialogStore from "../../../../store/DialogStore";
import { useEffect } from "react";
import useAxiosPrivate from "../../../../hooks/useAxiosPrivate";
import { observer } from "mobx-react-lite"

const DialogItemList = observer(() => {
    const axios = useAxiosPrivate();
    const { dialogs, getDialogs} = DialogStore;
    const { setMessages } = DialogStore;

    useEffect(() => {
        getDialogs(axios)
    }, [])

    
    const handleClickDialog = (event, axios, userId) => {
        setMessages(axios, userId);
    }
    console.log(dialogs)
    return (
        
        <Box sx={{height: '700px', overflowY: 'auto'}}>
            {dialogs.length == 0 && <Box>You, not have dialogs</Box>}
            {dialogs.map((x, i) =>
                <DialogItem userId={x.id} userName={x.username} lastMessage={x.lastMessage} sendTime={x.sendTime} key={i}/>
            )}
        </Box>
    )
})
export default DialogItemList;