import React, { useState } from 'react';
import {
    Dialog,
    DialogTitle,
    DialogContent,
    DialogActions,
    TextField,
    List,
    ListItem,
    ListItemText,
    Button,
    IconButton,
} from '@mui/material';
import PersonAddAltIcon from '@mui/icons-material/PersonAddAlt';
import CloseIcon from '@mui/icons-material/Close';
import { getUsersToInvite, addUsersToGroup } from '../../../../../services/group.service';
import { useEffect } from 'react';
import useAxiosPrivate from "../../../../../hooks/useAxiosPrivate";
import DialogStore from '../../../../../store/DialogStore';
const AddPersonToGroupWindow = () => {
    const axios = useAxiosPrivate();
    const { openedDialog } = DialogStore;
    const [open, setOpen] = useState(false);
    const [searchQuery, setSearchQuery] = useState('');
    const [users, setUsers] = useState([]);

    useEffect(() => {
        const getUsers = async () => {
            var usersToInvite = (await getUsersToInvite(axios, openedDialog, searchQuery === '' ? null : searchQuery)).data
            
            console.log(usersToInvite)
            setUsers(usersToInvite);
            console.log(users);
        }
        getUsers();
    }, [searchQuery])
    const [selectedUsers, setSelectedUsers] = useState([]);

    console.log(selectedUsers)

    const handleClickOpen = () => {
        setOpen(true);
    };

    const handleClose = () => {
        setOpen(false);
    };

    const handleSearchChange = (e) => {
        setSearchQuery(e.target.value);
    };

    const handleUserSelect = (userId) => {
        if (selectedUsers.includes(userId)) {
            setSelectedUsers(selectedUsers.filter(selectedUser => selectedUser !== userId));
        } else {
            setSelectedUsers([...selectedUsers, userId]);
        }
    };

    const handleAddClick = () => {
        addUsersToGroup(axios, openedDialog, selectedUsers);
        console.log('Users added:', selectedUsers);
        handleClose();
    };

    const isSelected = (user) => selectedUsers.includes(user);

    return (
        <div>
            <PersonAddAltIcon color="action" onClick={handleClickOpen} />

            <Dialog open={open} onClose={handleClose} maxWidth="md" fullWidth>
                <DialogTitle>
                    Add Person to Group
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
                    <TextField
                        autoFocus
                        margin="dense"
                        label="Search users..."
                        type="text"
                        fullWidth
                        variant="standard"
                        value={searchQuery}
                        onChange={handleSearchChange}
                    />
                    <List>
                        {users.length > 0 && users.map(user => (
                            <ListItem
                                key={user.Id}
                                button
                                selected={isSelected(user.id)}
                                onClick={() => handleUserSelect(user.id)}
                            >
                                <ListItemText primary={user.firstName + ' ' + user.secondName} secondary={user.username} />
                            </ListItem>
                        ))}
                    </List>
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleClose}>Cancel</Button>
                    <Button onClick={handleAddClick}>Add</Button>
                </DialogActions>
            </Dialog>
        </div>
    );
};

export default AddPersonToGroupWindow;