import React, { useState } from 'react';
import { Button, Container, Typography, Box } from '@mui/material';
import { AuthContext } from '../../../providers/AuthProvider';
import { useContext } from 'react';
import { jwtDecode } from 'jwt-decode';
import DialogStore from '../../../store/DialogStore';
import { observer } from 'mobx-react-lite';

const FileUploader = observer(({socket}) => {
  const [file, setFile] = useState(null);
  const [fileBytes, setFileBytes] = useState([]);
  const { openedDialog } = DialogStore;
  const [user, setUser] = useContext(AuthContext);
  const myId = jwtDecode(user.access_token)["Id"];
  const chunkSize = 100 * 1024;

  const handleFileChange = async (e) => {
    const selectedFile = e.target.files[0];
    setFile(selectedFile);

    const reader = new FileReader();
    reader.onloadend = (event) => {
      var bytes = new Int8Array(event.target.result);
      console.log(bytes);
      setFileBytes(bytes);
    };
    reader.readAsArrayBuffer(selectedFile);
  };

  const handleUpload = () => {
    if (!file) return;

    const parts = file.name.split(".");
    const fileExtension = parts[parts.length - 1];

    let messageObj = {
      "HeaderInfo": {
        "From" : myId,
        "To" : openedDialog,
        "Type": 1,
        "Content": 2,
        "SendTime": new Date().toISOString(),
        "Method" : "CreateFile"
      },
      "MessageContent": {
        "OriginalName": file.name
      }
    }

    socket.current.send(JSON.stringify(messageObj));

    let start = 0;
    let end = 0;

    while(start < fileBytes.length) {
      end = Math.min(start + chunkSize, fileBytes.length);
      console.log(start, end)
      socket.current.send(fileBytes.slice(start, end));
      start = end;
    }
    socket.current.send([]);

    // Сбросить выбранный файл
    setFile(null);
  };

  return (
    <Container>
      {openedDialog !== null && (
        <Box sx={{ marginTop: 2, display: 'flex', flexDirection: 'row', alignItems: 'center' }}>
          <Typography variant="h6" gutterBottom>
            Upload a File
          </Typography>
          <label htmlFor="file-input" style={{ marginLeft: '16px' }}>
            <Button component="span" variant="outlined" color="primary">
              Choose File
            </Button>
          </label>
          <input
            id="file-input"
            type="file"
            style={{ display: 'none' }}
            onChange={handleFileChange}
          />
          {file && <Typography variant="body1" style={{ marginLeft: '16px' }}>{file.name}</Typography>}
          <Button
            variant="contained"
            color="primary"
            onClick={handleUpload}
            disabled={!file}
            style={{ marginLeft: '16px' }}
          >
            Upload
          </Button>
        </Box>
      )}
    </Container>
  );
});

export default FileUploader;
