import React from 'react';
import { TextField, Button, Box } from '@mui/material';
import { AuthContext } from '../../../providers/AuthProvider';
import { useContext } from 'react';
import { jwtDecode } from 'jwt-decode';
import { observer } from "mobx-react-lite"
import DialogStore from '../../../store/DialogStore';

const InputField = observer(({ socket, handleMessageChange }) => {
  const { openedDialog, openedGroup, messageType } = DialogStore;
  const [user, setUser] = useContext(AuthContext);
  const myId = jwtDecode(user.access_token)["Id"];

  const sendMessage = (event) => {
    event.preventDefault();
    const data = new FormData(event.currentTarget);
    if (socket.current && socket.current.readyState === socket.current.OPEN) {
      let messageObj = {
        "HeaderInfo": {
          "From" : myId,
          "To" : openedDialog,
          "Type": messageType + 1,
          "Content": 1,
          "SendTime": new Date().toISOString(),
          "Method" : "CreateMessage"
        },
        "MessageContent": {
          "Content" : data.get("message")
        }
      }
      console.log("send message")
      console.log(messageObj)
      socket.current.send(JSON.stringify(messageObj));
    } else {
      console.error('WebSocket not open to send message');
    }
  }

  return (
    <Box
      component='form'
      onSubmit={sendMessage}
      sx={{
        display: 'flex',
        alignItems: 'center',
        marginTop: '16px', // Add margin at the top
      }}
    >
      {openedDialog !== null && (
        <Box sx={{ display: 'flex', alignItems: 'center', width: '100%' }}>
          <TextField
            label="Введите сообщение"
            variant="outlined"
            id="message"
            name="message"
            autoFocus
            sx={{ flex: 1, marginRight: '8px', width: '100%' }} // Add width: 100%
          />
          <Button variant="contained" type='submit' color="primary">
            Отправить
          </Button>
        </Box>
      )}
    </Box>
  );
});

export default InputField;
