import React, { useState } from "react";
import { Avatar, Box, Typography, Menu, MenuItem } from "@mui/material";
import DialogStore from "../../../../store/DialogStore";
import Icon from '@mui/material/Icon';
import DescriptionIcon from "@mui/icons-material/Description"

import { observer } from "mobx-react-lite";

const FileMessage = observer(({ message, myId, downloadFile, deleteMessage }) => {
  const [menuOpen, setMenuOpen] = useState(false);
  const [anchorEl, setAnchorEl] = useState(null);
  const [openModal, setOpenModal] = useState(false);

  const { updateMessageContent, deleteMessageContent } = DialogStore

  const [currentMessage, setCurrentMessage] = useState(message.content);

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
        <Typography variant="body1">{currentMessage}</Typography>
        <Typography variant="caption">{message.sendTime}</Typography>
        <Typography variant="caption">{message.messageContentType}</Typography>
      </Box>
      {menuOpen && (
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
          {isMyMessage && (<MenuItem onClick={handleDelete}>❌ Удалить</MenuItem>) }
          <MenuItem onClick={handleDowloadFile}>Download</MenuItem>

        </Menu>
      )}
      
    </Box>
  );
});

export default FileMessage;
