import React, { useState } from 'react';
import { Button, Dialog, DialogTitle, DialogContent, DialogActions, TextField } from '@mui/material';
import { createGroup } from '../../../../../services/group.service';
import useAxiosPrivate from '../../../../../hooks/useAxiosPrivate';
import DialogStore from '../../../../../store/DialogStore';
const CreateGroupWindow = () => {
    const axios = useAxiosPrivate();
    const [open, setOpen] = useState(false);
    const { addGroup } = DialogStore;
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
        var id = await createGroup(axios, inputValue);
        addGroup({
            id: id,
            name : inputValue,
            lastMessage : "",
            sendTime : ""
        });
        // Add your logic for form submission
        handleClose(); // Close the dialog after submission
    };

    return (
        <div>
            <Button onClick={handleOpen}>Create group</Button>
            <Dialog open={open} onClose={handleClose}>
                <DialogTitle>Group creating</DialogTitle>
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

export default CreateGroupWindow;