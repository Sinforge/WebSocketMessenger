import React from "react";
import Box from "@mui/material/Box"
import Avatar from "@mui/material/Avatar"
import Typography from "@mui/material/Typography"
import Grid from "@mui/material/Grid"
import DialogStore from "../../../../store/DialogStore";
import useAxiosPrivate from "../../../../hooks/useAxiosPrivate";

const options = { year: 'numeric', month: 'long', day: 'numeric', hour: '2-digit', minute: '2-digit', hour12: true };

const DialogItem = ({userId, userName, lastMessage, sendTime}) => {
    const axios = useAxiosPrivate();
    const { setMessages } = DialogStore;

    const handleClick = () => {
        setMessages(axios, userId);
    }

    return (
        <Box onClick={handleClick} sx={{
            marginBottom: 2,
            marginLeft: 3,
            display: 'flex',
            flexDirection: 'row',
            alignItems: 'center',
            cursor: 'pointer',
            padding: '10px',
            borderRadius: '5px',
            boxShadow: '0px 0px 10px rgba(0, 0, 0, 0.1)',
            transition: 'background-color 0.3s ease',
            '&:hover': {
                backgroundColor: '#f0f0f0',
            }
        }}>
            <Grid container spacing={2} alignItems="center">
                <Grid item xs={2} sm={1}>
                    <Avatar />
                </Grid>
                <Grid item xs={3} sm={4}>
                    <Typography variant="subtitle1" sx={{ fontWeight: 600, marginLeft: 2, fontSize: '0.9rem' }}>{userName}</Typography>
                </Grid>
                <Grid item xs={6} sm={5}>
                    <Typography variant="body1" sx={{ fontSize: '0.9rem', overflow: 'hidden', textOverflow: 'ellipsis' }}>{lastMessage}</Typography>
                </Grid>
                <Grid item xs={12} sm={3}>
                    <Typography variant="body2" color="textSecondary" sx={{ fontSize: '0.8rem' }}>
                        {sendTime === null || sendTime === undefined ? null : new Intl.DateTimeFormat('en-US', options).format(new Date(sendTime))}
                    </Typography>
                </Grid>
            </Grid>
        </Box>
    )
}

export default DialogItem;
