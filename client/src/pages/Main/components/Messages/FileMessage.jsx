import React, { useState } from "react";
import { Avatar, Box, Typography, Menu, MenuItem } from "@mui/material";
import DescriptionIcon from "@mui/icons-material/Description"
import DialogStore from "../../../../store/DialogStore";

import { observer } from "mobx-react-lite";

const options = { year: 'numeric', month: 'long', day: 'numeric', hour: '2-digit', minute: '2-digit', hour12: true };


const FileMessage = observer(({ message, myId, downloadFile, deleteMessage }) => {
  const [menuOpen, setMenuOpen] = useState(false);
  const [anchorEl, setAnchorEl] = useState(null);
  const [openModal, setOpenModal] = useState(false);
  const { groupRoleId, messageType} = DialogStore;
  const [currentMessage, setCurrentMessage] = useState(message.content);

  console.log(groupRoleId, messageType);
  const isMyMessage = message.authorId === myId;

  const handleOpenMenu = (event) => {
    if (!menuOpen) {
      setAnchorEl(event.currentTarget);
      setMenuOpen(true);
    } else {
      handleCloseMenu();
    }
  };

  const handleCloseMenu = () => {
    setAnchorEl(null);
    setMenuOpen(false);
  };

  const handleUpdate = () => {
    setOpenModal(true);
    handleCloseMenu();
  };

  const handleCloseModal = () => {
    setOpenModal(false);
  };


  const handleDelete = () => {
    deleteMessage(message.id);
    handleCloseMenu();
  };

  const handleDowloadFile = () => {
    downloadFile(message.id,  message.content);
    handleCloseMenu();
  }

  const handleTextClick = () => {
    setMenuOpen(!menuOpen);
  };

  return (
    <Box
      sx={{
        display: "flex",
        alignItems: "center",
        padding: "10px",
        width: "fit-content",
        border: "1px solid #ccc",
        borderRadius: "5px",
        marginBottom: "10px",
        backgroundColor: isMyMessage ? "#e6f7ff" : "#f0f0f0",
        marginLeft: isMyMessage ? "0" : "auto",
        position: "relative", // чтобы положение меню было относительно сообщения
      }}
    >
      <Avatar />
      <Box
        sx={{ 
          marginLeft: "10px",
          flexGrow: 1,
          cursor: "pointer" // Добавляем стиль, чтобы указать, что текст можно кликнуть
        }}
        onClick={handleTextClick}
      >
        <DescriptionIcon/>
        <Typography variant="caption">{message.username}</Typography>
        <Typography variant="body1">
          {currentMessage.length > 26 && message.messageContentType === 2 ?
          `${currentMessage.substring(0, 15)}...${currentMessage.substring(currentMessage.length - 10)}`
          : currentMessage}
        </Typography>
        <Typography variant="caption">{new Intl.DateTimeFormat('en-US', options).format(new Date(message.sendTime))}</Typography>
        <Typography variant="caption">{message.messageContentType}</Typography>
      </Box>
      {menuOpen &&  (
        <Menu
          anchorEl={anchorEl}
          open={menuOpen}
          onClose={handleCloseMenu}
          anchorOrigin={{
            vertical: "top",
            horizontal: "right",
          }}
          transformOrigin={{
            vertical: "top",
            horizontal: "right",
          }}
        >
          {(isMyMessage || messageType === 1 && groupRoleId <= 2) && (<MenuItem onClick={handleDelete}>❌ Удалить</MenuItem>) }
          <MenuItem onClick={handleDowloadFile}>Download</MenuItem>

        </Menu>
      )}
      
    </Box>
  );
});

export default FileMessage;
