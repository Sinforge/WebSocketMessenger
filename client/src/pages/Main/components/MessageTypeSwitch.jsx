import Typography from '@mui/material/Typography';
import { useState } from 'react';
import { Box, styled } from '@mui/material';
import DialogStore from '../../../store/DialogStore';

const MessageTypeSwitch = () => {
    const { messageType, setMessageType} = DialogStore;
    const handleSwitch = (type) => {
        console.log(type);
        setMessageType(type);
    };

    const StyledSpan = styled('span')(({ theme, isActive }) => ({
        display: 'inline-block',
        padding: theme.spacing(1),
        cursor: 'pointer',
        color: isActive ? theme.palette.primary.main : theme.palette.text.secondary,
        '&:hover': {
            color: isActive ? theme.palette.primary.dark : theme.palette.text.secondary,
        },
    }));

    return (
        <Box display="flex" justifyContent="center">
            <StyledSpan isActive={messageType === 0} onClick={() => handleSwitch(0)}>
                <Typography>Personal</Typography>
            </StyledSpan>
            <StyledSpan isActive={messageType === 1} onClick={() => handleSwitch(1)}>
                <Typography>Groups</Typography>
            </StyledSpan>
        </Box>
    );
};

export default MessageTypeSwitch;