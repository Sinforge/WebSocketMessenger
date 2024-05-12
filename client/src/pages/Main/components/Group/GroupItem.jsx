import React from "react";
import Box from "@mui/material/Box"
import Avatar from "@mui/material/Avatar"
import Typography from "@mui/material/Typography"
import Grid from "@mui/material/Grid"
import DialogStore from "../../../../store/DialogStore";
import useAxiosPrivate from "../../../../hooks/useAxiosPrivate";
import GroupIcon from '@mui/icons-material/Group';

const options = { year: 'numeric', month: 'long', day: 'numeric', hour: '2-digit', minute: '2-digit', hour12: true };

const GroupItem = ({groupId, groupName, lastMessage, sendTime}) => {
    const axios = useAxiosPrivate();
    const { setGroupMessages } = DialogStore;

    const handleClick = () => {
        setGroupMessages(axios, groupId );
    }

    console.log(groupId, groupName, lastMessage, sendTime)
    return (
        <Box onClick={handleClick} sx={{
            marginBottom: 4,
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
                    <Avatar>
                        <GroupIcon/>
                    </Avatar>
                </Grid>
                <Grid item xs={3} sm={4}>
                    <Typography variant="subtitle1" sx={{ fontWeight: 600, marginLeft: 2 }}>{groupName}</Typography>
                </Grid>
                <Grid item xs={6} sm={5}>
                    <Typography variant="body1">{lastMessage}</Typography>
                </Grid>
                <Grid item xs={12} sm={3}>
                    <Typography variant="body2" color="textSecondary">
                        {sendTime === null || sendTime === undefined || sendTime === "0001-01-01T00:00:00" ? "" : new Intl.DateTimeFormat('en-US', options).format(new Date(sendTime))}
                    </Typography>
                </Grid>
            </Grid>
        </Box>
    )
}

export default GroupItem;
