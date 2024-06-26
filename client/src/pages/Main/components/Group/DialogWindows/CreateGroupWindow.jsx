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
        var id = (await createGroup(axios, inputValue)).data;
        addGroup({
            id: id,
            name : inputValue,
            lastMessage : null,
            sendTime : null
        });
        handleClose();
    };

    return (
        <div>
            <Button onClick={handleOpen} variant="contained" color="primary">Create group</Button>
            <Dialog open={open} onClose={handleClose}>
                <DialogTitle>Group creating</DialogTitle>
                <DialogContent>
                    <TextField
                        label="Enter group name"
                        value={inputValue}
                        onChange={handleInputChange}
                        fullWidth
                    />
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleClose} color="secondary">Cancel</Button>
                    <Button onClick={handleSubmit} color="primary">Submit</Button>
                </DialogActions>
            </Dialog>
        </div>
    );
};

export default CreateGroupWindow;