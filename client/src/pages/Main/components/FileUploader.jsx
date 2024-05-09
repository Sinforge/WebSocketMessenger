import React, { useState } from 'react';
import { Button, Container, Grid, Typography } from '@mui/material';
import { AuthContext } from '../../../providers/AuthProvider';
import { useContext } from 'react';
import { jwtDecode } from 'jwt-decode';
import DialogStore from '../../../store/DialogStore';
const FileUploader = ({socket}) => {
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

    // Split the string by "." to separate the file name and extension
    const parts = file.name.split(".");

    // Get the last part of the split array, which will be the file extension
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
    
    // // Simulate file upload delay (remove in production)
    // setTimeout(() => {
    //   // Send base64String to JSON endpoint
    //   // You can use fetch or any HTTP client library to make the request
    //   console.log('Uploading file:', base64String);
    // }, 1000);
  };

  return (
    <Container>
      <Grid container spacing={2} alignItems="center">
        <Grid item xs={12}>
          <Typography variant="h6" gutterBottom>
            Upload a File
          </Typography>
        </Grid>
        <Grid item xs={12}>
          <input
            accept="image/*" // Set accepted file types if necessary
            id="file-input"
            type="file"
            style={{ display: 'none' }}
            onChange={handleFileChange}
          />
          <label htmlFor="file-input">
            <Button component="span" variant="outlined" color="primary">
              Choose File
            </Button>
          </label>
          {file && <Typography variant="body1">{file.name}</Typography>}
        </Grid>
        <Grid item xs={12}>
          <Button
            variant="contained"
            color="primary"
            onClick={handleUpload}
            disabled={!file}
          >
            Upload
          </Button>
        </Grid>
      </Grid>
    </Container>
  );
};

export default FileUploader;