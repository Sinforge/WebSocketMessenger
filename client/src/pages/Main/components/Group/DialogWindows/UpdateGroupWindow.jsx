import React, { useState } from 'react';
import { Button, Dialog, DialogTitle, DialogContent, DialogActions, TextField } from '@mui/material';
import { updateGroup } from '../../../../../services/group.service';
import useAxiosPrivate from '../../../../../hooks/useAxiosPrivate';
import DialogStore from '../../../../../store/DialogStore';
import SettingsIcon from '@mui/icons-material/Settings';
const UpdateGroupWindow = () => {
    const axios = useAxiosPrivate();
    const [open, setOpen] = useState(false);
    const { addGroup, openedDialog, updateGroupLocaly } = DialogStore;
    const [inputValue, setInputValue] = useState('');

    const handleOpen = () => {
        setOpen(true);
    };

    const handleClose = () => {
        setOpen(false);
    };

    const handleInputChange = (event) => {
        setInputValue(event.target.value);
    };

    const handleSubmit = async () => {
        // Handle form submission here
        await updateGroup(axios, openedDialog, inputValue);
        updateGroupLocaly(openedDialog, inputValue);
        // Add your logic for form submission
        handleClose(); // Close the dialog after submission
    };

    return (
        <div>
            <SettingsIcon onClick={handleOpen}/>
            <Dialog open={open} onClose={handleClose}>
                <DialogTitle>Group updating</DialogTitle>
                <DialogContent>
                    <TextField
                        label="Enter group name"
                        value={inputValue}
                        onChange={handleInputChange}
                    />
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleClose}>Cancel</Button>
                    <Button onClick={handleSubmit}>Submit</Button>
                </DialogActions>
            </Dialog>
        </div>
    );
};

export default UpdateGroupWindow;