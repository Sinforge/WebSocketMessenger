import React from "react";
import { observer } from "mobx-react-lite"
import DialogStore from "../../../../store/DialogStore";
import UpdateGroupWindow from "./DialogWindows/UpdateGroupWindow";
import AddPersonToGroupWindow from "./DialogWindows/AddPersonToGroupWindow";
import GetMembersOfGroupWindow from "./DialogWindows/GetMembersOfGroupWindow";
import LogoutIcon from '@mui/icons-material/Logout';
import useAxiosPrivate from "../../../../hooks/useAxiosPrivate";
import { jwtDecode } from 'jwt-decode';
import { AuthContext } from "../../../../providers/AuthProvider";
import { deleteUserFromGroup } from "../../../../services/group.service";
import { useContext } from "react";

const GroupHeader = observer(() => {
    const { openedGroup, openedDialog, deleteGroup } = DialogStore;
    const axios = useAxiosPrivate();
    const [user, setUser] = useContext(AuthContext);
    const myId = jwtDecode(user.access_token)["Id"];

    const handleLeave = () => {
        deleteGroup(openedDialog)
        deleteUserFromGroup(axios, openedDialog, myId);
    }

    const groupMenuStyle = {
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'space-between',
        padding: '10px',
        backgroundColor: '#f0f0f0',
        border: '1px solid #ccc',
        borderRadius: '5px',
    };

    return(
        <div>
            {openedGroup !== null && (
                 <div style={groupMenuStyle}>
                    <div className="group-name">{openedGroup.name}</div>
                    <AddPersonToGroupWindow/>
                    <UpdateGroupWindow/>
                    <GetMembersOfGroupWindow/>
                    <LogoutIcon onClick={handleLeave}/>
                </div>
            )
            }

        </div>
    )
})

export default GroupHeader;