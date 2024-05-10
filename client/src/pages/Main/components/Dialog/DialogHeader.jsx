import React from "react";
import { observer } from "mobx-react-lite";
import DialogStore from "../../../../store/DialogStore";

const DialogHeader = observer(() => {
    const { openedUser } = DialogStore;

    const headerStyle = {
        backgroundColor: "#f0f0f0",
        padding: "10px",
        borderBottom: "1px solid #ccc",
        borderRadius: "5px",
        marginBottom: "10px",
    };

    return (
        <div style={headerStyle}>
            {openedUser !== null && (
                <div>{openedUser.username}</div>
            )}
        </div>
    );
});

export default DialogHeader;