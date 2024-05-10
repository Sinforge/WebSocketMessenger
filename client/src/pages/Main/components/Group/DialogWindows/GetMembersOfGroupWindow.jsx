import React, { useState } from 'react';
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
} from '@mui/material';
import GroupsIcon from '@mui/icons-material/Groups';

import CloseIcon from '@mui/icons-material/Close';
import DeleteIcon from '@mui/icons-material/Delete';

const GetMembersOfGroupWindow = () => {
    const [open, setOpen] = useState(false);
    const [users, setUsers] = useState([
        { id: 1, name: 'John Doe', role: 'Admin' },
        { id: 2, name: 'Jane Smith', role: 'Moderator' },
        { id: 3, name: 'Bob Johnson', role: 'User' },
        { id: 4, name: 'Alice Williams', role: 'Admin' },
        { id: 5, name: 'Tom Davis', role: 'Moderator' },
    ]);

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
                                <ListItemText primary={user.name} secondary={user.role} />
                                <IconButton
                                    aria-label="remove"
                                    onClick={() => handleRemoveUser(user)}
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