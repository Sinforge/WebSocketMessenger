import React from "react";
import { observer } from "mobx-react-lite"
import DialogStore from "../../../store/DialogStore";
const DialogHeader = observer(() => {
    const { openedUser, openedDialog, dialogs } = DialogStore;
    return(
        <div>
            {openedUser !== null && (
                <div>Dialogs with {openedUser.username}</div>
            )
            }

        </div>
    )
})

export default DialogHeader;