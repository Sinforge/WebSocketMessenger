import React, { useContext, useState } from 'react';
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
    Select,
    MenuItem,
} from '@mui/material';
import GroupsIcon from '@mui/icons-material/Groups';
import CloseIcon from '@mui/icons-material/Close';
import DeleteIcon from '@mui/icons-material/PersonRemove';
import useAxiosPrivate from '../../../../../hooks/useAxiosPrivate';
import { useEffect } from 'react';
import DialogStore from '../../../../../store/DialogStore';
import { AuthContext } from '../../../../../providers/AuthProvider';
import { jwtDecode } from 'jwt-decode';
import { updateUserGroupRole, deleteUserFromGroup } from '../../../../../services/group.service';
const GetMembersOfGroupWindow = () => {
    const [open, setOpen] = useState(false);
    const [users, setUsers] = useState([]);
    const { openedDialog, setGroupRole } = DialogStore;
    
    const [user, setUser] = useContext(AuthContext);
    const myId = jwtDecode(user.access_token)["Id"];
    const [currentUserRole, setCurrentUserRole] = useState('')
    const axios = useAxiosPrivate();


    useEffect(() => {
        const getUsers = async () => {
            var members = (await getGroupMembers(axios, openedDialog)).data
            var roleId = members.find(x => x.id === myId).roleId;
            setCurrentUserRole(roleId);
            setGroupRole(roleId);
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

    const handleRemoveUser = (userId) => {
        // Simulate removing the user
        deleteUserFromGroup(axios, openedDialog, userId);
        setUsers(prevUsers => prevUsers.filter(user => user.id !== userId));
        console.log(`Removing user ${userId}`);
    };

    const handleRoleChange = (userId, newRoleId) => {
        updateUserGroupRole(axios, openedDialog, userId, newRoleId);
        setUsers(prevUsers => prevUsers.map(user => {
            if (user.id === userId) {
                return { ...user, roleId: newRoleId };
            }
            return user;
        }));
        console.log(`Changing role for user ${userId} to ${newRoleId}`);
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
                                        {"Role: "}
                                        { currentUserRole <= 2 && user.roleId !== 1 ? (
                                            <Select
                                                value={user.roleId}
                                                onChange={(event) => handleRoleChange(user.id, event.target.value)}
                                            >
                                                <MenuItem value={2}>Moderator</MenuItem>
                                                <MenuItem value={3}>User</MenuItem>
                                                {/* Add more roles as needed */}
                                            </Select>
                                        ) : (
                                            user.roleName // Если пользователь создатель, просто отображаем текущую роль
                                        )}
                                        </Typography>
                                    </React.Fragment>
                                } />
                                <IconButton
                                    aria-label="remove"
                                    onClick={() => handleRemoveUser(user.id)}
                                >
                                    { currentUserRole <= 2 && user.id !== myId &&
                                    <DeleteIcon />
                                    }
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