import React, { useState } from "react";
import Box from "@mui/material/Box"
import DialogStore from "../../../../store/DialogStore";
import { useEffect } from "react";
import useAxiosPrivate from "../../../../hooks/useAxiosPrivate";
import { observer } from "mobx-react-lite"
import GroupItem from "./GroupItem";

const GroupItemList = observer(() => {
    const axios = useAxiosPrivate();
    const { dialogs, getGroups} = DialogStore;

    useEffect(() => {
        getGroups(axios)
    }, [])


    return (
        
        <Box sx={{height: '700px', overflowY: 'auto'}}>
            {dialogs.length == 0 && <Box>You, not have groups</Box>}
            {dialogs.map((x, i) =>
                <GroupItem groupId={x.id} groupName={x.name} lastMessage={x.lastMessage} sendTime={x.sendTime} key={i}/>
            )}
        </Box>
    )
})
export default GroupItemList;