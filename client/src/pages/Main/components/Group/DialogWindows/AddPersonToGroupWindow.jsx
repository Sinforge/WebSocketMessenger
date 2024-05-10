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

const AddPersonToGroupWindow = () => {
    const [open, setOpen] = useState(false);
    const [searchQuery, setSearchQuery] = useState('');
    const [users] = useState([
        { id: 1, name: 'John Doe' },
        { id: 2, name: 'Jane Smith' },
        { id: 3, name: 'Bob Johnson' },
        { id: 4, name: 'Alice Williams' },
        { id: 5, name: 'Tom Davis' },
    ]);
    const [selectedUsers, setSelectedUsers] = useState([]);

    const filteredUsers = users.filter(user =>
        user.name.toLowerCase().includes(searchQuery.toLowerCase())
    );

    const handleClickOpen = () => {
        setOpen(true);
    };

    const handleClose = () => {
        setOpen(false);
    };

    const handleSearchChange = (e) => {
        setSearchQuery(e.target.value);
    };

    const handleUserSelect = (user) => {
        if (selectedUsers.includes(user)) {
            setSelectedUsers(selectedUsers.filter(selectedUser => selectedUser !== user));
        } else {
            setSelectedUsers([...selectedUsers, user]);
        }
    };

    const handleAddClick = () => {
        // Simulate adding selected users
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
                        {filteredUsers.map(user => (
                            <ListItem
                                key={user.id}
                                button
                                selected={isSelected(user)}
                                onClick={() => handleUserSelect(user)}
                            >
                                <ListItemText primary={user.name} />
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