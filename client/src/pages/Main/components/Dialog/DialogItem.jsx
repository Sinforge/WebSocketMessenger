import React from "react";
import Box from "@mui/material/Box"
import Avatar from "@mui/material/Avatar"
import Typography from "@mui/material/Typography"
import Grid from "@mui/material/Grid"
import DialogStore from "../../../../store/DialogStore";
import useAxiosPrivate from "../../../../hooks/useAxiosPrivate";
const DialogItem = ({userId, userName, lastMessage, sendTime}) => {
    const axios = useAxiosPrivate();
    const { setMessages } = DialogStore;
    const handleClick = () => {
        setMessages(axios, userId);

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
                    <Typography>{userName}</Typography>
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
export default DialogItem;