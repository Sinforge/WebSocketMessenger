import React from "react";
import { observer } from "mobx-react-lite"
import DialogStore from "../../../../store/DialogStore";
import GroupsIcon from '@mui/icons-material/Groups';
import AddPersonToGroupWindow from "./DialogWindows/AddPersonToGroupWindow";
import GetMembersOfGroupWindow from "./DialogWindows/GetMembersOfGroupWindow";
const GroupHeader = observer(() => {
    const { openedGroup } = DialogStore;

    const groupMenuStyle = {
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'space-between',
        padding: '10px',
        backgroundColor: '#f0f0f0',
        border: '1px solid #ccc',
        borderRadius: '5px',
    };

    const handleIconClick = () => {
        // Add your icon click functionality here
    };


    return(
        <div>
            {openedGroup !== null && (
                 <div style={groupMenuStyle}>
                    <div className="group-name">{openedGroup.name}</div>
                    <AddPersonToGroupWindow/>
                    <GetMembersOfGroupWindow/>
                </div>
            )
            }

        </div>
    )
})

export default GroupHeader;