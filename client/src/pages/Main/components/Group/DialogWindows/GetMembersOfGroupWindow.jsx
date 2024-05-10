import React, { useState } from 'react';
import { getGroupMembers } from '../../../../../services/group.service';
import {
    Dialog,
    DialogTitle,
    DialogContent,
    DialogActions,
    List,
    ListItem,
    ListItemText,
    IconButton,
    Button,
    Typography,
} from '@mui/material';
import GroupsIcon from '@mui/icons-material/Groups';
import CloseIcon from '@mui/icons-material/Close';
import DeleteIcon from '@mui/icons-material/Delete';
import useAxiosPrivate from '../../../../../hooks/useAxiosPrivate';
import { useEffect } from 'react';
import DialogStore from '../../../../../store/DialogStore';
const GetMembersOfGroupWindow = () => {
    const [open, setOpen] = useState(false);
    const [users, setUsers] = useState([]);
    const { openedDialog } = DialogStore;
    const axios = useAxiosPrivate();


    useEffect(() => {
        const getUsers = async () => {
            var members = (await getGroupMembers(axios, openedDialog)).data
            
            setUsers(members);
        }
        getUsers();
    }, [])
    const handleClickOpen = () => {
        setOpen(true);
    };

    const handleClose = () => {
        setOpen(false);
    };

    const handleRemoveUser = (user) => {
        // Simulate removing the user
        console.log(`Removing user ${user.name}`);
    };
    const iconStyle = {
        fontSize: '24px',
        marginLeft: '10px',
        cursor: 'pointer',
    };

    return (
        <div>
            <GroupsIcon style={iconStyle} onClick={handleClickOpen} />
            <Dialog open={open} onClose={handleClose} maxWidth="lg" fullWidth>
                <DialogTitle>
                    Members of Group
                    <IconButton
                        aria-label="close"
                        onClick={handleClose}
                        sx={{
                            position: 'absolute',
                            right: 8,
                            top: 8,
                            color: theme => theme.palette.grey[500],
                        }}
                    >
                        <CloseIcon />
                    </IconButton>
                </DialogTitle>
                <DialogContent>
                    <List>
                        {users.map((user) => (
                            <ListItem key={user.id}>
                                <ListItemText primary={user.firstName + ' ' + user.secondName} 
                                secondary={
                                    <React.Fragment>
                                        <Typography component="span" variant="body2" color="textPrimary">
                                            {"Username: " + user.username}
                                        </Typography>
                                        <br />
                                        <Typography component="span" variant="body2" color="textSecondary">
                                            {"Role: " + user.roleName}
                                        </Typography>
                                    </React.Fragment>
                                } />
                                <IconButton
                                    aria-label="remove"
                                    onClick={() => handleRemoveUser(user.id)}
                                >
                                    <DeleteIcon />
                                </IconButton>
                            </ListItem>
                        ))}
                    </List>
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleClose}>Cancel</Button>
                </DialogActions>
            </Dialog>
        </div>
    );
};

export default GetMembersOfGroupWindow;