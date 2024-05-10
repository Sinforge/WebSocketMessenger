import React from "react";
import Box from "@mui/material/Box"
import Avatar from "@mui/material/Avatar"
import Typography from "@mui/material/Typography"
import Grid from "@mui/material/Grid"
import DialogStore from "../../../../store/DialogStore";
import useAxiosPrivate from "../../../../hooks/useAxiosPrivate";
const GroupItem = ({groupId, groupName, lastMessage, sendTime}) => {
    const axios = useAxiosPrivate();
    const { setGroupMessages } = DialogStore;
    const handleClick = () => {
        setGroupMessages(axios, groupId );
    }

    return (
        <Box onClick={handleClick} sx={{
            marginBottom: 4,
            display: 'flex',
            flexDirection: 'row',
        }}>
            <Grid container spacing={0.2}>
                <Grid xs={6} sm={1}>
                    <Avatar sx={{
                        marginRight: 2
                    }}/>
                </Grid>
                <Grid xs={6} sm={6}>
                    <Typography>{groupName}</Typography>
                </Grid>
                <Grid xs={6}>
                    <Typography>{lastMessage}</Typography>
                </Grid>
                <Grid xs={6}>
                    <Typography>{sendTime}</Typography>
                </Grid>
            </Grid>
           

        </Box>
    )
}
export default GroupItem;